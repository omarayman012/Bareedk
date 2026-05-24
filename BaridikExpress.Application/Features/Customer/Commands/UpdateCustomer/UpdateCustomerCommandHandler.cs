using BaridikExpress.Application.Features.Customer.Dtos;
using BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage,
    ISender mediator)
    : IRequestHandler<UpdateCustomerCommand, Result<CustomerDetailsResponse>>
{
    #region Handle

    public async Task<Result<CustomerDetailsResponse>> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Customer

        var customer = await db.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<CustomerDetailsResponse>.Failure(localizer["CustomerNotFound"], 404);

        #endregion

        #region Validate Contacts (if sent)

        if (request.Contacts is { Count: > 0 })
        {
            var primaryContact = request.Contacts.FirstOrDefault(x => x.IsPrimary == true);

            if (primaryContact is null)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["PrimaryContactIsRequired"]);

            var contactDuplicate = await db.Customers
                .Where(c => c.Id != request.Id)
                .Where(c => c.Contacts.Any(x =>
                    x.Email == primaryContact.Email ||
                    x.PhoneNumber == primaryContact.PhoneNumber ||
                    x.WhatsAppNumber == primaryContact.WhatsAppNumber))
                .SelectMany(c => c.Contacts)
                .Where(x =>
                    x.Email == primaryContact.Email ||
                    x.PhoneNumber == primaryContact.PhoneNumber ||
                    x.WhatsAppNumber == primaryContact.WhatsAppNumber)
                .Select(x => new { x.Email, x.PhoneNumber, x.WhatsAppNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (contactDuplicate?.Email == primaryContact.Email)
                return Result<CustomerDetailsResponse>.Failure(localizer["EmailAlreadyExists"]);

            if (contactDuplicate?.PhoneNumber == primaryContact.PhoneNumber)
                return Result<CustomerDetailsResponse>.Failure(localizer["PhoneNumberAlreadyExists"]);

            if (contactDuplicate?.WhatsAppNumber == primaryContact.WhatsAppNumber)
                return Result<CustomerDetailsResponse>.Failure(localizer["WhatsAppNumberAlreadyExists"]);
        }

        #endregion

        #region Validate Career Field & Nationality (if sent)

        if (request.CareerFieldId.HasValue)
        {
            var exists = await db.CareerFields
                .AnyAsync(x => x.Id == request.CareerFieldId.Value, cancellationToken);

            if (!exists)
                return Result<CustomerDetailsResponse>.Failure(localizer["CareerFieldNotFound"]);
        }

        if (request.NationalityId.HasValue)
        {
            var exists = await db.Nationalities
                .AnyAsync(x => x.Id == request.NationalityId.Value, cancellationToken);

            if (!exists)
                return Result<CustomerDetailsResponse>.Failure(localizer["NationalityNotFound"]);
        }

        #endregion

        #region Validate Addresses (if sent)

        if (request.Addresses is { Count: > 0 })
        {
            var addressError = await ValidateAddressesAsync(request.Addresses, cancellationToken);
            if (addressError is not null)
                return Result<CustomerDetailsResponse>.Failure(addressError);
        }

        #endregion

        #region Validate Tax Registration Number (if sent)

        if (request.Account?.TaxRegistrationNumber is not null)
        {
            var taxExists = await db.CustomerAccounts
                .AnyAsync(x =>
                    x.CustomerId != request.Id &&
                    x.TaxRegistrationNumber == request.Account.TaxRegistrationNumber,
                    cancellationToken);

            if (taxExists)
                return Result<CustomerDetailsResponse>
                    .Failure(localizer["TaxRegistrationNumberAlreadyExists"]);
        }

        #endregion

        #region Upload Image (if sent)

        var imageUrl = customer.ImageUrl;

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

        #region Update (Transaction)

        await using var transaction = await db.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            #region Update Customer Core

            customer.Update(
                request.Name ?? customer.Name,
                request.NationalityId ?? customer.NationalityId,
                request.CareerFieldId ?? customer.CareerFieldId,
                imageUrl);

            #endregion

            #region Update Contacts (if sent)

            if (request.Contacts is { Count: > 0 })
            {
                var existingContactIds = customer.Contacts.Select(c => c.Id).ToHashSet();
                var incomingContactIds = request.Contacts
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value)
                    .ToHashSet();

                var contactsToDelete = customer.Contacts
                    .Where(c => !incomingContactIds.Contains(c.Id))
                    .ToList();

                db.CustomerContacts.RemoveRange(contactsToDelete);

                foreach (var dto in request.Contacts)
                {
                    if (dto.Id.HasValue && existingContactIds.Contains(dto.Id.Value))
                    {
                        var existing = customer.Contacts.First(c => c.Id == dto.Id.Value);
                        existing.Update(
                            dto.PhoneCountryCode,
                            dto.PhoneNumber,
                            dto.Email,
                            dto.WhatsAppCountryCode,
                            dto.WhatsAppNumber,
                            dto.IsPrimary);
                    }
                    else
                    {
                        var newContact = CustomerContact.Create(
                            customer.Id,
                            dto.PhoneCountryCode ?? string.Empty,
                            dto.PhoneNumber ?? string.Empty,
                            dto.Email ?? string.Empty,
                            dto.WhatsAppCountryCode,
                            dto.WhatsAppNumber,
                            dto.IsPrimary ?? false);

                        await db.CustomerContacts.AddAsync(newContact, cancellationToken);
                    }
                }
            }

            #endregion

            #region Update Addresses (if sent)

            if (request.Addresses is { Count: > 0 })
            {
                var existingAddressIds = customer.Addresses.Select(a => a.Id).ToHashSet();
                var incomingAddressIds = request.Addresses
                    .Where(a => a.Id.HasValue)
                    .Select(a => a.Id!.Value)
                    .ToHashSet();

                var addressesToDelete = customer.Addresses
                    .Where(a => !incomingAddressIds.Contains(a.Id))
                    .ToList();

                db.CustomerAddresses.RemoveRange(addressesToDelete);

                foreach (var dto in request.Addresses)
                {
                    if (dto.Id.HasValue && existingAddressIds.Contains(dto.Id.Value))
                    {
                        var existing = customer.Addresses.First(a => a.Id == dto.Id.Value);
                        existing.Update(
                            dto.AddressType,
                            dto.CountryId,
                            dto.GovernmentId,
                            dto.CityId,
                            dto.VillageId,
                            dto.Street,
                            dto.BuildingNumber,
                            dto.FloorNumber,
                            dto.DistinctiveMark,
                            dto.ZipCode,
                            dto.Location,
                            dto.IsDefault);
                    }
                    else
                    {
                        var newAddress = CustomerAddress.Create(
                            customer.Id,
                            dto.AddressType,
                            dto.CountryId,
                            dto.GovernmentId,
                            dto.CityId,
                            dto.VillageId,
                            dto.Street,
                            dto.BuildingNumber,
                            dto.FloorNumber,
                            dto.DistinctiveMark,
                            dto.ZipCode,
                            dto.Location,
                            dto.IsDefault ?? false);

                        await db.CustomerAddresses.AddAsync(newAddress, cancellationToken);
                    }
                }
            }

            #endregion

            #region Update Account (if sent)

            if (request.Account is not null)
            {
                if (customer.Account is not null)
                {
                    customer.Account.Update(
                        request.Account.TaxRegistrationNumber,
                        request.Account.Currency,
                        request.Account.OpeningBalance,
                        request.Account.OpeningBalanceDate,
                        request.Account.Note);
                }
                else
                {
                    var newAccount = CustomerAccount.Create(
                        customer.Id,
                        request.Account.TaxRegistrationNumber,
                        request.Account.Currency,
                        request.Account.OpeningBalance,
                        request.Account.OpeningBalanceDate,
                        request.Account.Note);

                    await db.CustomerAccounts.AddAsync(newAccount, cancellationToken);
                }
            }

            #endregion

            #region Save & Commit

            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            #endregion

            return await mediator.Send(
                new GetCustomerByIdQuery(customer.Id), cancellationToken);
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
        IList<UpdateCustomerAddressDto> addresses,
        CancellationToken cancellationToken)
    {
        var countryIds = addresses.Where(a => a.CountryId.HasValue).Select(a => a.CountryId!.Value).Distinct().ToList();
        var governmentIds = addresses.Where(a => a.GovernmentId.HasValue).Select(a => a.GovernmentId!.Value).Distinct().ToList();
        var cityIds = addresses.Where(a => a.CityId.HasValue).Select(a => a.CityId!.Value).Distinct().ToList();
        var villageIds = addresses.Where(a => a.VillageId.HasValue).Select(a => a.VillageId!.Value).Distinct().ToList();

        var validCountries = countryIds.Count > 0 ? await db.Countries.Where(x => countryIds.Contains(x.CountryId)).Select(x => x.CountryId).ToListAsync(cancellationToken) : [];
        var validGovernments = governmentIds.Count > 0 ? await db.Governments.Where(x => governmentIds.Contains(x.GovernmentId)).Select(x => x.GovernmentId).ToListAsync(cancellationToken) : [];
        var validCities = cityIds.Count > 0 ? await db.Cities.Where(x => cityIds.Contains(x.CityId)).Select(x => x.CityId).ToListAsync(cancellationToken) : [];
        var validVillages = villageIds.Count > 0 ? await db.Villages.Where(x => villageIds.Contains(x.VillageId)).Select(x => x.VillageId).ToListAsync(cancellationToken) : [];

        if (countryIds.Except(validCountries).Any()) return localizer["CountryNotFound"];
        if (governmentIds.Except(validGovernments).Any()) return localizer["GovernmentNotFound"];
        if (cityIds.Except(validCities).Any()) return localizer["CityNotFound"];
        if (villageIds.Except(validVillages).Any()) return localizer["VillageNotFound"];

        return null;
    }

    #endregion
}