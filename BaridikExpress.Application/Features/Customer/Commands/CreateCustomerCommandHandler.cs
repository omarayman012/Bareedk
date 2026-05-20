using BaridikExpress.Application.Features.Customer.Dtos;
using BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Commands;

public sealed class CreateCustomerCommandHandler
    (
    IApplicationDbContext applicationDb,
    UserManager<User> userManager,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateCustomerCommand, Result<CustomerDetailsResponse>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<Result<CustomerDetailsResponse>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var primaryContact = request.Contacts.FirstOrDefault(x => x.IsPrimary);

        if (primaryContact is null)
            return Result<CustomerDetailsResponse>
                .Failure(_localizer["PrimaryContactIsRequired"]);

        var emailIsExist = await _applicationDb.Customers
            .AnyAsync(x => x.Contacts.Any(c => c.Email == primaryContact.Email),
                cancellationToken);

        var userEmailIsExist = await _userManager.Users
            .AnyAsync(u => u.Email == primaryContact.Email,
                cancellationToken);

        var phoneIsExist = await _applicationDb.Customers
            .AnyAsync(x => x.Contacts.Any(c => c.PhoneNumber == primaryContact.PhoneNumber),
                cancellationToken);

        var userPhoneIsExist = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == primaryContact.PhoneNumber,
                cancellationToken);

        var whatsappIsExist = await _applicationDb.Customers
            .AnyAsync(x => x.Contacts.Any(c => c.WhatsAppNumber == primaryContact.WhatsAppNumber),
                cancellationToken);

        if (emailIsExist || userEmailIsExist)
            return Result<CustomerDetailsResponse>
                .Failure(_localizer["EmailAlreadyExists."]);

        if (phoneIsExist || userPhoneIsExist)
            return Result<CustomerDetailsResponse>
                .Failure(_localizer["PhoneNumberAlreadyExists."]);

        if (whatsappIsExist)
            return Result<CustomerDetailsResponse>
                .Failure(_localizer["WhatsAppNumberAlreadyExists."]);

        if (request.CareerFieldId.HasValue)
        {
            var careerFieldExists = await _applicationDb.CareerFields
                .AnyAsync(x => x.Id == request.CareerFieldId.Value,
                    cancellationToken);

            if (!careerFieldExists)
                return Result<CustomerDetailsResponse>
                    .Failure(_localizer["CareerFieldNotFound"]);
        }

        if (request.NationalityId.HasValue)
        {
            var nationalityExists = await _applicationDb.Nationalities
                .AnyAsync(x => x.Id == request.NationalityId.Value,
                    cancellationToken);

            if (!nationalityExists)
                return Result<CustomerDetailsResponse>
                    .Failure(_localizer["NationalityNotFound"]);
        }

        if (request.Addresses is not null)
        {
            foreach (var address in request.Addresses)
            {
                if (address.CountryId.HasValue)
                {
                    var countryExists = await _applicationDb.Countries
                        .AnyAsync(x => x.CountryId == address.CountryId.Value,
                            cancellationToken);

                    if (!countryExists)
                        return Result<CustomerDetailsResponse>
                            .Failure(_localizer["CountryNotFound"]);
                }

                if (address.GovernmentId.HasValue)
                {
                    var governmentExists = await _applicationDb.Governments
                        .AnyAsync(x => x.GovernmentId == address.GovernmentId.Value,
                            cancellationToken);

                    if (!governmentExists)
                        return Result<CustomerDetailsResponse>
                            .Failure(_localizer["GovernmentNotFound"]);
                }

                if (address.CityId.HasValue)
                {
                    var cityExists = await _applicationDb.Cities
                        .AnyAsync(x => x.CityId == address.CityId.Value,
                            cancellationToken);

                    if (!cityExists)
                        return Result<CustomerDetailsResponse>
                            .Failure(_localizer["CityNotFound"]);
                }

                if (address.VillageId.HasValue)
                {
                    var villageExists = await _applicationDb.Villages
                        .AnyAsync(x => x.VillageId == address.VillageId.Value,
                            cancellationToken);

                    if (!villageExists)
                        return Result<CustomerDetailsResponse>
                            .Failure(_localizer["VillageNotFound"]);
                }
            }
        }

        string? imageUrl = null;

        if (request.Image != null)
        {
            var fileExtension = Path.GetExtension(request.Image.FileName);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            imageUrl = await _fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "customer-images");

            if (imageUrl is null)
                return Result<CustomerDetailsResponse>
                    .Failure(_localizer["ImageUploadFailed"], 400);
        }

        var user = new User
        {
            FullName = request.Name,
            ProfileImageUrl = imageUrl,
            UserName = primaryContact.Email,
            Email = primaryContact.Email,
            PhoneNumber = primaryContact.PhoneNumber,
            EmailConfirmed = true,
        };

        var createUserResult = await _userManager
            .CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = string.Join(", ",
                createUserResult.Errors.Select(e => e.Description));

            return Result<CustomerDetailsResponse>
                .Failure(_localizer["FailedToCreateUser."] + " " + errors);
        }

        var customer = Domain.Entities.Customers.Customer.Create(
            user.Id,
            request.Name,
            request.NationalityId,
            request.CareerFieldId,
            imageUrl);

        await _applicationDb.Customers
            .AddAsync(customer, cancellationToken);

        foreach (var contactItem in request.Contacts)
        {
            var contact = CustomerContact.Create(
                customer.Id,
                contactItem.PhoneCountryCode,
                contactItem.PhoneNumber,
                contactItem.Email,
                contactItem.WhatsAppCountryCode,
                contactItem.WhatsAppNumber,
                contactItem.IsPrimary);

            await _applicationDb.CustomerContacts
                .AddAsync(contact, cancellationToken);
        }

        if (request.Addresses is not null)
        {
            foreach (var addressItem in request.Addresses)
            {
                var address = CustomerAddress.Create(
                    customer.Id,
                    addressItem.AddressType,
                    addressItem.CountryId,
                    addressItem.GovernmentId,
                    addressItem.CityId,
                    addressItem.VillageId,
                    addressItem.Street,
                    addressItem.BuildingNumber,
                    addressItem.FloorNumber,
                    addressItem.DistinctiveMark,
                    addressItem.ZipCode,
                    addressItem.Location,
                    false);

                await _applicationDb.CustomerAddresses
                    .AddAsync(address, cancellationToken);
            }
        }

        if (request.Account is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.Account.TaxRegistrationNumber))
            {
                var taxExists = await _applicationDb.CustomerAccounts
                    .AnyAsync(x => x.TaxRegistrationNumber == request.Account.TaxRegistrationNumber,
                        cancellationToken);

                if (taxExists)
                    return Result<CustomerDetailsResponse>
                        .Failure(_localizer["TaxRegistrationNumberAlreadyExists"]);
            }

            var account = CustomerAccount.Create(
                customer.Id,
                request.Account.TaxRegistrationNumber,
                request.Account.Currency,
                request.Account.OpeningBalance,
                request.Account.OpeningBalanceDate,
                request.Account.Note);

            await _applicationDb.CustomerAccounts
                .AddAsync(account, cancellationToken);
        }

        await _applicationDb.SaveChangesAsync(cancellationToken);
        var query = new GetCustomerByIdQuery(customer.Id);
        var handler = new GetCustomerByIdHandler(_applicationDb, _localizer);
        return await handler.Handle(query, cancellationToken);
    }
}