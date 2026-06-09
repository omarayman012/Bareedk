using BaridikExpress.Application.Features.DeliveryTypes.DTO;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;

public record CreateDeliveryTypeCommand(
    string? NameEn,
    string ?NameAr,
    int DaysFrom,
    int DaysTo,
    decimal PricePerShipment,
        Currency Currency,

    bool IsSwitchActive,
    IFormFile? Image,
    string? DescriptionEn,
    string? DescriptionAr
) : IRequest<Result<DeliveryTypeResponse>>;