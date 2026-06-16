using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetById;

public sealed record GetServiceBusinessPlanByIdQuery(Guid Id)
: IRequest<Result<ServiceBusinessPlanResponse>>;
