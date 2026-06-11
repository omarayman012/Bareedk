using BaridikExpress.Application.Features.Announcements.DTO;

namespace BaridikExpress.Application.Features.Announcements.Queries.GetAnnouncementById
{
    public record GetAnnouncementByIdQuery(Guid Id)
        : IRequest<Result<GetAnnouncementByIdDto>>;
}
