namespace BaridikExpress.Application.Features.Vehicles.Commands.ToggleVehicleStatus
{
    public record ToggleVehicleStatusCommand(Guid Id)
        : IRequest<Result<bool>>;
}