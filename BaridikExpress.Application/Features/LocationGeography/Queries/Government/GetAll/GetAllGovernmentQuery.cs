using BaridikExpress.Application.Features.LocationGeography.Dto.Government;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetAll;

public class GetAllGovernmentQuery
    : IRequest<Result<PaginatedList<GovernmentDto>>>
{
    public string? Name { get; set; }

    public Guid? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}