using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetAll;


public class GetAllGovernmentQuery
    : BaseFilter, IRequest<Result<PaginatedList<GovernmentDto>>>
{
    public string? Name { get; set; }
    public Guid? CountryId { get; set; }
}