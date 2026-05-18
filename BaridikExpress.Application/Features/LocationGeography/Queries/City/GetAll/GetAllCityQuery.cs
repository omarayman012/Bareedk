using BaridikExpress.Application.Features.LocationGeography.Dto.City;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetAll;

public class GetAllCityQuery
    : IRequest<Result<PaginatedList<CityDto>>>
{
    public string? Name { get; set; }

    public Guid? GovernmentId { get; set; }

    public Guid? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}