namespace BaridikExpress.Application.Features.Announcements.Commands.CreateAnnouncement;

public record CreateAnnouncementCommand(
    string TitleAr,
    string TitleEn,
    string TextColor,
    string BackgroundColor) : IRequest<Result<Guid>>;