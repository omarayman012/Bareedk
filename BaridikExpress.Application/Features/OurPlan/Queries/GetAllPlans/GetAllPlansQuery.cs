
using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.OurPlans.DTO;

namespace BaridikExpress.Application.Features.OurPlans.Queries.GetAllPlans;
public class GetAllPlansQuery
    : BaseFilter ,
      IRequest<Result<PaginatedList<GetAllPlansDto>>>
{
    public string? Name { get; init; }
}
