using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Vehicles.Commands.UpdateVehicles
{
    public record UpdateVehicleCommand(
        Guid Id,
        string ?NameAr,
        string ?NameEn,
        decimal ?LoadCapacityFrom,
        decimal ?LoadCapacityTo,
        decimal ?PricePerTon,
        Currency ?Currency,
        IFormFile? ImageUrl,
        bool IsPriceCalculationEnabled
    ) : IRequest<Result<bool>>;
}