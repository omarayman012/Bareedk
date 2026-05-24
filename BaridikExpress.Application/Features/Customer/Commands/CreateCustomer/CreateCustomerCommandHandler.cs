using System.Linq;
using BaridikExpress.Application.Features.Customer.Dtos;
using BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Customers;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Application.Features.Customer.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler(
    IApplicationDbContext applicationDb,
    UserManager<User> userManager,
    IStringLocalizer localizer,
    IFileStorageService fileStorage,
    ISender mediator)
    : IRequestHandler<CreateCustomerCommand, Result<CustomerDetailsResponse>>
{
    #region Handle

    public async Task<Result<CustomerDetailsResponse>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Primary Contact

        var primaryContact = request.Contacts.FirstOrDefault(x => x.IsPrimary);

        if (primaryContact is null)
            return Result<CustomerDetailsResponse>
                .Failure(localizer["PrimaryContactIsRequired"]);

        #endregion

        #region Validate Uniqueness (Email, Phone, WhatsApp)

        var contactDuplicate = await applicationDb.Customers
            .Where(x => x.Contacts.Any(c =>
                c.Email == primaryContact.Email ||
                c.PhoneNumber == primaryContact.PhoneNumber ||
                c.WhatsAppNumber == primaryContact.WhatsAppNumber))
            .SelectMany(x => x.Contacts)
            .Where(c =>
                c.Email == primaryContact.Email ||
                c.PhoneNumber == primaryContact.PhoneNumber ||
                c.WhatsAppNumber == primaryContact.WhatsAppNumber)
            .Select(c => new
            {
                c.Email,
                c.PhoneNumber,
                c.WhatsAppNumber
            })
            .FirstOrDefaultAsync(cancellationToken);

        var userDuplicate = await userManager.Users
            .Where(u =>
                u.Email == primaryContact.Email ||
                u.PhoneNumber == primaryContact.PhoneNumber)
            .Select(u => new { u.Email, u.PhoneNumber })
            .FirstOrDefaultAsync(cancellationToken);

        if (contactDuplicate?.Email == primaryContact.Email || userDuplicate?.Email == primaryContact.Email)
            return Result<CustomerDetailsResponse>
                .Failure(localizer["EmailAlreadyExists"]);

        if (contactDuplicate?.PhoneNumber == primaryContact.PhoneNumber || userDuplicate?.PhoneNumber == primaryContact.PhoneNumber)
            return Result<CustomerDetailsResponse>
                .Failure(localizer["PhoneNumberAlreadyExists"]);

        if (contactDuplicate?.WhatsAppNumber == primaryContact.WhatsAppNumber)
            return Result<CustomerDetailsResponse>
                .Failure(localizer["WhatsAppNumberAlreadyExists"]);

        #endregion

        #region Validate Career Field & Nationality

        if (request.CareerFieldId.HasValue)
        {
            var exists = await applicationDb.CareerFields
                .AnyAsync(x => x.Id == request.CareerFieldId.Value, cancellationToken);

            if (!exists)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["CareerFieldNotFound"]);
        }

        if (request.NationalityId.HasValue)
        {
            var exists = await applicationDb.Nationalities
                .AnyAsync(x => x.Id == request.NationalityId.Value, cancellationToken);

            if (!exists)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["NationalityNotFound"]);
        }

        #endregion

        #region Validate Addresses

        if (request.Addresses is { Count: > 0 })
        {
            var validationError = await ValidateAddressesAsync(request.Addresses, cancellationToken);

            if (validationError is not null)
                return Result<CustomerDetailsResponse>.Failure(validationError);
        }

        #endregion

        #region Validate Tax Registration Number

        if (request.Account is not null &&
            !string.IsNullOrWhiteSpace(request.Account.TaxRegistrationNumber))
        {
            var taxExists = await applicationDb.CustomerAccounts
                .AnyAsync(x => x.TaxRegistrationNumber == request.Account.TaxRegistrationNumber,
                    cancellationToken);

            if (taxExists)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["TaxRegistrationNumberAlreadyExists"]);
        }

        #endregion

        #region Upload Image

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "customer-images");

            if (imageUrl is null)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Create User, Customer & Related Entities (Transaction)

        await using var transaction = await applicationDb.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            #region Create Identity User

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.Name,
                ProfileImageUrl = imageUrl,
                UserName = primaryContact.Email,
                NormalizedUserName = primaryContact.Email.ToUpper(),
                Email = primaryContact.Email,
                NormalizedEmail = primaryContact.Email.ToUpper(),
                PhoneNumber = primaryContact.PhoneNumber,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, request.Password);

            await applicationDb.ApplicationUsers.AddAsync(user, cancellationToken);
            await applicationDb.SaveChangesAsync(cancellationToken);

            #endregion

            #region Create Customer
            var customer = Domain.Entities.Customers.Customer.Create(
                request.Name,  
                user.Id,       
                request.NationalityId,
                request.CareerFieldId,
                imageUrl);

            await applicationDb.Customers.AddAsync(customer, cancellationToken);
            await applicationDb.SaveChangesAsync(cancellationToken);

            #endregion

            #region Create Contacts

            var contacts = request.Contacts
                .Select(c => CustomerContact.Create(
                    customer.Id,
                    c.PhoneCountryCode,
                    c.PhoneNumber,
                    c.Email!,
                    c.WhatsAppCountryCode,
                    c.WhatsAppNumber,
                    c.IsPrimary))
                .ToList();

            await applicationDb.CustomerContacts.AddRangeAsync(contacts, cancellationToken);

            #endregion

            #region Create Addresses

            if (request.Addresses is { Count: > 0 })
            {
                var addresses = request.Addresses
                    .Select(a => CustomerAddress.Create(
                        customer.Id,
                        a.AddressType,
                        a.CountryId,
                        a.GovernmentId,
                        a.CityId,
                        a.VillageId,
                        a.Street,
                        a.BuildingNumber,
                        a.FloorNumber,
                        a.DistinctiveMark,
                        a.ZipCode,
                        a.Location,
                        false))
                    .ToList();

                await applicationDb.CustomerAddresses.AddRangeAsync(addresses, cancellationToken);
            }

            #endregion

            #region Create Account

            if (request.Account is not null)
            {
                var account = CustomerAccount.Create(
                    customer.Id,
                    request.Account.TaxRegistrationNumber,
                    request.Account.Currency,
                    request.Account.OpeningBalance,
                    request.Account.OpeningBalanceDate,
                    request.Account.Note);

                await applicationDb.CustomerAccounts.AddAsync(account, cancellationToken);
            }

            #endregion

            #region Save & Commit

            await applicationDb.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            #endregion

            return await mediator.Send(new GetCustomerByIdQuery(customer.Id), cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        #endregion
    }

    #endregion

    #region Validate Addresses Helper

    private async Task<string?> ValidateAddressesAsync(
        IList<CreateAddressDto> addresses,
        CancellationToken cancellationToken)
    {
        var countryIds = addresses
            .Where(a => a.CountryId.HasValue)
            .Select(a => a.CountryId!.Value)
            .Distinct()
            .ToList();

        var governmentIds = addresses
            .Where(a => a.GovernmentId.HasValue)
            .Select(a => a.GovernmentId!.Value)
            .Distinct()
            .ToList();

        var cityIds = addresses
            .Where(a => a.CityId.HasValue)
            .Select(a => a.CityId!.Value)
            .Distinct()
            .ToList();

        var villageIds = addresses
            .Where(a => a.VillageId.HasValue)
            .Select(a => a.VillageId!.Value)
            .Distinct()
            .ToList();

        var validCountries = countryIds.Count > 0
            ? await applicationDb.Countries
                .Where(x => countryIds.Contains(x.CountryId))
                .Select(x => x.CountryId)
                .ToListAsync(cancellationToken)
            : [];

        var validGovernments = governmentIds.Count > 0
            ? await applicationDb.Governments
                .Where(x => governmentIds.Contains(x.GovernmentId))
                .Select(x => x.GovernmentId)
                .ToListAsync(cancellationToken)
            : [];

        var validCities = cityIds.Count > 0
            ? await applicationDb.Cities
                .Where(x => cityIds.Contains(x.CityId))
                .Select(x => x.CityId)
                .ToListAsync(cancellationToken)
            : [];

        var validVillages = villageIds.Count > 0
            ? await applicationDb.Villages
                .Where(x => villageIds.Contains(x.VillageId))
                .Select(x => x.VillageId)
                .ToListAsync(cancellationToken)
            : [];

        if (countryIds.Except(validCountries).Any()) return localizer["CountryNotFound"];
        if (governmentIds.Except(validGovernments).Any()) return localizer["GovernmentNotFound"];
        if (cityIds.Except(validCities).Any()) return localizer["CityNotFound"];
        if (villageIds.Except(validVillages).Any()) return localizer["VillageNotFound"];

        return null;
    }

    #endregion
}