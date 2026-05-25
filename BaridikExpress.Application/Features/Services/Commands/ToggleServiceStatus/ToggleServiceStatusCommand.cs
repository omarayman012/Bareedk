namespace BaridikExpress.Application.Features.Services.Commands.ToggleServiceStatus;

public sealed record ToggleServiceStatusCommand(Guid Id)
    : IRequest<Result<bool>>;