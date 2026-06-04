using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.LocationGeography.Dto.City;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetAll;

public class GetAllCityQuery : BaseFilter, IRequest<Result<PaginatedList<CityDto>>>
{
    public string? Name { get; set; }
    public Guid? GovernmentId { get; set; }
    public Guid? CountryId { get; set; }
}