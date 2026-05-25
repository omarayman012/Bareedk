using BaridikExpress.Application.Features.Services.DTOs;

namespace BaridikExpress.Application.Features.Services.Queries.GetServiceById;

public sealed record GetServiceByIdQuery(Guid Id)
    : IRequest<Result<ServiceResponse>>;