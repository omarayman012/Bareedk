using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Vehicles.Commands.CreateVehicles
{
    public record CreateVehicleCommand(
        string NameAr,
        string NameEn,
        decimal LoadCapacityFrom,
        decimal LoadCapacityTo,
        decimal PricePerTon,
        Currency Currency,
       IFormFile ImageUrl,
        bool IsPriceCalculationEnabled
    ) : IRequest<Result<CreateVehicleResponse>>;
}