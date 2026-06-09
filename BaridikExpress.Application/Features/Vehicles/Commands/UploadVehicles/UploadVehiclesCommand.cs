using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.Vehicles.Commands.UploadVehicles
{
    public record UploadVehiclesCommand(IFormFile File) 
        : IRequest<Result<ExcelUploadResult<Vehicle>>>;
}
