using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.Vehicles.Commands.ToggleVehicleStatus
{
    public class ToggleVehicleStatusCommandHandler(
        IGenericRepository<Vehicle> repo,
        IStringLocalizer localizer
    ) : IRequestHandler<ToggleVehicleStatusCommand, Result<bool>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<bool>> Handle(
            ToggleVehicleStatusCommand request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _repo.GetByIdAsync(request.Id);

            if (vehicle is null)
                return Result<bool>.Failure(
                    _localizer["VehicleNotFound"]);
            vehicle.ToggleStatus();
            await _repo.UpdateAsync(vehicle);

            return Result<bool>.Success(
               true,
                _localizer["OperationCompletedSuccessfully"]);
        }
    }
}