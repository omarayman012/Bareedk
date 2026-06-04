using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    Guid Id,
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
    bool? IsDefault
) : IRequest<Result<bool>>;
