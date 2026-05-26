using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.DeliveryTypes.DTO;

public sealed record DeliveryTypeResponse(
    Guid Id,
    LocalizedDto Name,
    int DaysFrom,
    int DaysTo,
    decimal PricePerShipment,
    decimal PricePerTotal,
    bool IsSwitchActive,
    string? ImageUrl,
    LocalizedDto? Description,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);