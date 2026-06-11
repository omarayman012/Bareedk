using BaridikExpress.Application.Features.Announcements.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncementsMobile
{

    public sealed record GetAllAnnouncementsMobileQuery(
        int PageNumber = 1,
        int PageSize = 10)
        : IRequest<Result<PaginatedList<GetAllAnnouncementsMobileDto>>>;
}
