using BaridikExpress.Application.Features.CustomerAddresses.DTOs;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.CustomerAddresses.Commands.CreateAddress;

public record CreateAddressCommand(
    AddressType? AddressType,
    string? RecipientName,
    string? Email,
    string? PhoneNumber,
    string? AddressTitle,
    Guid? CountryId,
    Guid? GovernmentId,
    Guid? CityId,
    Guid? VillageId,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? FloorNumber,
    string? DistinctiveMark,
    string? ZipCode,
    decimal? Latitude,
    decimal? Longitude,
    bool IsDefault = false
) : IRequest<Result<Guid>>;
