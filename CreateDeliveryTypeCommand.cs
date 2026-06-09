BaridikExpress.Application\Features\DeliveryTypes\Commands\CreateDeliveryType\CreateDeliveryTypeCommand.cs
using BaridikExpress.Domain.Enum;
using BaridikExpress.Application.Features.DeliveryTypes.DTO;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;
public record CreateDeliveryTypeCommand(
    string? NameEn,
    string? NameAr,
    int DaysFrom,
    int DaysTo,
    decimal PricePerShipment,
    bool IsSwitchActive,
    Currency Currency,
    IFormFile? Image,
    string? DescriptionEn,
    string? DescriptionAr
) : IRequest<Result<DeliveryTypeResponse>>;