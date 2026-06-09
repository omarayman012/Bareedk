using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Banners.DTO;

namespace BaridikExpress.Application.Features.Banners.Queries.GetAllBanners;

public class GetAllBannersQuery: BaseFilter,IRequest<Result<PaginatedList<GetAllBannersDto>>>
{
    public string? Name { get; set; }= string.Empty;

}