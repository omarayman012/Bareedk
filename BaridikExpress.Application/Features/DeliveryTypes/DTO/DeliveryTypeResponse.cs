using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.DeliveryTypes.DTO;
public sealed record DeliveryTypeResponse(
    Guid Id,
    LocalizedDto Name,
    int DaysFrom,
    int DaysTo,
    decimal PricePerShipment,
    decimal PricePerTotal,
    Currency Currency,
    bool IsSwitchActive,
    string? ImageUrl,
    LocalizedDto? Description,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
)
{
    public string PricePerShipmentWithCurrency => $"{PricePerShipment} {Currency}";
    public string PricePerTotalWithCurrency => $"{PricePerTotal} {Currency}";
}