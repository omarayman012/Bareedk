
namespace BaridikExpress.Application.Features.OurPlans.Commands.TogglePlanStatus;
public record TogglePlanStatusCommand(Guid Id) : IRequest<Result<bool>>;
