namespace BaridikExpress.Application.Features.Announcements.Commands.DeleteAnnouncement;

public record DeleteAnnouncementCommand(List<Guid> Ids) : IRequest<Result<bool>>;
