using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Customer.Dtos;

public record CreateAddressDto(
    AddressType? AddressType,
    Guid? CountryId,
    Guid? GovernmentId,
    Guid? CityId,
    Guid? VillageId,
    string? Street,
    string? BuildingNumber,
    string? FloorNumber,
    string? DistinctiveMark,
    string? ZipCode,
    string? Location,
    bool IsDefault = true
);