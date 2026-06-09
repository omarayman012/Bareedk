using Microsoft.AspNetCore.Http;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.UpdateDeliveryType;

public sealed class UpdateDeliveryTypeCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public int? DaysFrom { get; set; }
    public int? DaysTo { get; set; }
    public decimal? PricePerShipment { get; set; }
    public bool? IsSwitchActive { get; set; }
    public Currency? Currency { get; set; }
    public IFormFile? Image { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
}