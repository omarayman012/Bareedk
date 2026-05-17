namespace BaridikExpress.Application.Features.CareerFields.Commands.ToggleCareerFieldStatus;

public record ToggleCareerFieldStatusCommand(
    Guid Id
) : IRequest<Result<bool>>;