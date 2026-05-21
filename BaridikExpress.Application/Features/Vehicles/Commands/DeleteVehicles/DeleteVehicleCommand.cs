namespace BaridikExpress.Application.Features.Vehicles.Commands.DeleteVehicles
{
    public record DeleteVehicleCommand(List<Guid> Ids)
        : IRequest<Result<bool>>;
}