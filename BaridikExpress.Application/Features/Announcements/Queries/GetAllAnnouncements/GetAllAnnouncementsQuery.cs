using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Announcements.DTO;

namespace BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncements;

public class GetAllAnnouncementsQuery: BaseFilter,IRequest<Result<PaginatedList<GetAllAnnouncementsDto>>>
{
    public string? Name { get; set; }= string.Empty;

}