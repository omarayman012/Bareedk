
namespace BaridikExpress.Application.Features.OurPlans.Commands.DeletePlan;
public record DeletePlanCommand(List<Guid> Ids) : IRequest<Result<bool>>;
