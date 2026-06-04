using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.Vehicles.Commands.CreateVehicles
{
    public class CreateVehicleCommandHandler(
        IGenericRepository<Vehicle> repo,
        IStringLocalizer localizer,
        IFileStorageService fileStorage
    ) : IRequestHandler<CreateVehicleCommand, Result<CreateVehicleResponse>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly IFileStorageService _fileStorage = fileStorage;
        public async Task<Result<CreateVehicleResponse>> Handle(
            CreateVehicleCommand request,
            CancellationToken cancellationToken)
        {
            var (nameAr, nameEn) = NormalizeHelper.Normalize(
                request.NameAr,
                request.NameEn);

            var exists = await _repo.AnyAsync(x =>
                x.NameEn == nameEn ||
                x.NameAr == nameAr);

            if (exists)
                return Result<CreateVehicleResponse>.Failure(
                    _localizer["VehicleAlreadyExists"]);

            var imagePath = string.Empty;
            if (request.ImageUrl is not null)
            {
                using var stream = request.ImageUrl.OpenReadStream();
               imagePath = await _fileStorage.SaveFileAsync(stream, request.ImageUrl.FileName, "Vehicle-images");
            }
            if(string.IsNullOrEmpty(imagePath))
                return Result<CreateVehicleResponse>.Failure(
                    _localizer["ImageUploadFailed"]);

            var vehicle = new Vehicle(
                nameEn,
                nameAr,
                request.LoadCapacityFrom,
                request.LoadCapacityTo,
                request.PricePerTon,
                request.Currency,
               imagePath,
                request.IsPriceCalculationEnabled);

            await _repo.AddAsync(vehicle);

            return Result<CreateVehicleResponse>.Success(
                new CreateVehicleResponse(
                    Id: vehicle.Id,
                    TotalPrice: vehicle.TotalPrice),
                _localizer["OperationCompletedSuccessfully"]);
        }
    }
}