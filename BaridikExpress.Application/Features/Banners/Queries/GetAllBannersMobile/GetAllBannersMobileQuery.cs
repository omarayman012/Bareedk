using BaridikExpress.Application.Features.Banners.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Banners.Queries.GetAllBannersMobile
{

    public sealed record GetAllBannersMobileQuery(
        int PageNumber = 1,
        int PageSize = 10)
        : IRequest<Result<PaginatedList<GetAllBannersMobileDto>>>;
}
