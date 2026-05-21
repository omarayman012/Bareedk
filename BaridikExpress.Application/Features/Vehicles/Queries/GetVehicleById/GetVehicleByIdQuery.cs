using BaridikExpress.Application.Features.Vehicles.DTO;

namespace BaridikExpress.Application.Features.Vehicles.Queries.GetVehicleById
{
    public record GetVehicleByIdQuery(Guid Id)
        : IRequest<Result<GetVehicleByIdDto>>;
}