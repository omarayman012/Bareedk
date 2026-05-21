using BaridikExpress.Application.Features.Vehicles.DTO;

namespace BaridikExpress.Application.Features.Vehicles.Commands.CreateVehicles
{
    public record CreateVehicleCommand(
        string NameAr,
        string NameEn,
        decimal LoadCapacityFrom,
        decimal LoadCapacityTo,
        decimal PricePerTon,
       IFormFile ImageUrl,
        bool IsPriceCalculationEnabled
    ) : IRequest<Result<CreateVehicleResponse>>;
}