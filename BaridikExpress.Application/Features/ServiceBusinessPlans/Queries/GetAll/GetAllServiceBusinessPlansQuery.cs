using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetAll;

public sealed class GetAllServiceBusinessPlansQuery
: BaseFilter,
IRequest<Result<PaginatedList<ServiceBusinessPlanResponse>>>
{
    public string? Name { get; set; }
}
