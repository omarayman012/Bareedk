namespace BaridikExpress.Application.Features.Services.Commands.DeleteServices;

public sealed record DeleteServicesCommand(List<Guid> Ids)
    : IRequest<Result<bool>>;