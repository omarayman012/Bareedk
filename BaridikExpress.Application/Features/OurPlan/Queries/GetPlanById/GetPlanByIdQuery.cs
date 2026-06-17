
using BaridikExpress.Application.Features.OurPlans.DTO;

namespace BaridikExpress.Application.Features.OurPlans.Queries.GetPlanById;
public record GetPlanByIdQuery(Guid Id) : IRequest<Result<GetPlanByIdDto>>;
