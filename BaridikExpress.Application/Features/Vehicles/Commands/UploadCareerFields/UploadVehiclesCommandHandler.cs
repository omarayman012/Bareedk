using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.Vehicles.Commands.UploadVehicles
{
    public class UploadVehiclesCommandHandler(
        IExcelService excelService,
        IApplicationDbContext context)
        : IRequestHandler<
            UploadVehiclesCommand,
            Result<ExcelUploadResult<Vehicle>>>
    {
        public async Task<Result<ExcelUploadResult<Vehicle>>> Handle(
            UploadVehiclesCommand request,
            CancellationToken cancellationToken)
        {
            var result = await excelService
                .UploadAsync<VehicleExcelDto, Vehicle>(
                    request.File,

                    dto => new Vehicle(
                        dto.NameEn,
                        dto.NameAr,
                        dto.LoadCapacityFrom,
                        dto.LoadCapacityTo,
                        dto.PricePerTon,
                       dto.Currency,
                        dto.ImageUrl,
                        dto.IsPriceCalculationEnabled),

                    async entity => await context.Vehicles
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.NameAr == entity.NameAr ||
                            x.NameEn == entity.NameEn,
                            cancellationToken),

                    entity => $"{entity.NameAr}|{entity.NameEn}",

                    cancellationToken);

            return Result<ExcelUploadResult<Vehicle>>
                .Success(result);
        }
    }
}