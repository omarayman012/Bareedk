using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetALL;

public class GetAllCountryQuery : IRequest<Result<PaginatedList<GetCountryResponse>>>
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public string? CreatedById { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}