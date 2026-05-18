using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetALL;

public class GetAllCountryQuery:IRequest<Result<PaginatedList<GetCountryResponse>>>
{
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

}
