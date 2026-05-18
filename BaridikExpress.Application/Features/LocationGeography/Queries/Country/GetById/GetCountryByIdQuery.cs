using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetById;

public class GetCountryByIdQuery:IRequest<Result<GetCountryResponse>>
{
    public Guid Id { get; set; }
}
