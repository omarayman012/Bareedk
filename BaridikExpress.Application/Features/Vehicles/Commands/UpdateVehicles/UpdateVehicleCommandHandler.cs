using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.Vehicles.Commands.UpdateVehicles
{
    public class UpdateVehicleCommandHandler(
        IGenericRepository<Vehicle> repo,
        IStringLocalizer localizer,
        IFileStorageService fileStorage
    ) : IRequestHandler<UpdateVehicleCommand, Result<bool>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly IFileStorageService _fileStorage = fileStorage;

        public async Task<Result<bool>> Handle(
            UpdateVehicleCommand request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _repo.GetByIdAsync(request.Id);

            if (vehicle is null)
            {
                return Result<bool>.Failure(
                    _localizer["VehicleNotFound"]);
            }

            string? imagePath = vehicle.ImageUrl;

            if (request.ImageUrl is not null)
            {
                using var stream = request.ImageUrl.OpenReadStream();

                imagePath = await _fileStorage.UpdateFileAsync(
                    stream,
                    request.ImageUrl.FileName,
                    vehicle.ImageUrl,
                    "Vehicle-images");

                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    return Result<bool>.Failure(
                        _localizer["ImageUploadFailed"]);
                }
            }

            var nameAr = request.NameAr ?? vehicle.NameAr;
            var nameEn = request.NameEn ?? vehicle.NameEn;
            (nameAr, nameEn) = NormalizeHelper.Normalize(
                nameAr,
                nameEn);

            vehicle.Update(
                nameEn,
                nameAr,
                request.LoadCapacityFrom,
                request.LoadCapacityTo,
                request.PricePerTon,
                request.Currency,
                imagePath,
                request.IsPriceCalculationEnabled);

            await _repo.UpdateAsync(vehicle);

            return Result<bool>.Success(
                true,
                _localizer["OperationCompletedSuccessfully"]);
        }
    }
}