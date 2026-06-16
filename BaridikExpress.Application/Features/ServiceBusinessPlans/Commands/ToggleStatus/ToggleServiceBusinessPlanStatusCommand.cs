namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.ToggleStatus;

public sealed record ToggleServiceBusinessPlanStatusCommand(Guid Id)
: IRequest<Result<bool>>;
