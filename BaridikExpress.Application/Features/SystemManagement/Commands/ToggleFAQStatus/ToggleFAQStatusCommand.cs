namespace BaridikExpress.Application.Features.SystemManagement.Commands.ToggleFAQStatus;

public sealed record ToggleFAQStatusCommand(
    Guid Id
) : IRequest<Result<bool>>;