using BaridikExpress.Application.Features.LocationGeography.Dto.City;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetById;

public class GetCityByIdQuery
    : IRequest<Result<CityDto>>
{
    public Guid Id { get; set; }
}