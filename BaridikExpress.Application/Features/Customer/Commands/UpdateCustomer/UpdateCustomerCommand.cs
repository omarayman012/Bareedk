using BaridikExpress.Application.Features.Customer.Dtos;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Customer.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommand : IRequest<Result<CustomerDetailsResponse>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? NationalityId { get; set; }
    public Guid? CareerFieldId { get; set; }
    public IFormFile? Image { get; set; }
    public List<UpdateCustomerContactDto>? Contacts { get; set; }
    public List<UpdateCustomerAddressDto>? Addresses { get; set; }
    public UpdateCustomerAccountDto? Account { get; set; }
}

public sealed class UpdateCustomerContactDto
{
    public Guid? Id { get; set; }
    public string? PhoneCountryCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? WhatsAppCountryCode { get; set; }
    public string? WhatsAppNumber { get; set; }
    public bool? IsPrimary { get; set; }
}

public sealed class UpdateCustomerAddressDto
{
    public Guid? Id { get; set; }
    public AddressType? AddressType { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? GovernmentId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? VillageId { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? FloorNumber { get; set; }
    public string? DistinctiveMark { get; set; }
    public string? ZipCode { get; set; }
    public string? Location { get; set; }
    public bool? IsDefault { get; set; }
}

public sealed class UpdateCustomerAccountDto
{
    public string? TaxRegistrationNumber { get; set; }
    public Currency? Currency { get; set; }
    public decimal? OpeningBalance { get; set; }
    public DateOnly? OpeningBalanceDate { get; set; }
    public string? Note { get; set; }
}