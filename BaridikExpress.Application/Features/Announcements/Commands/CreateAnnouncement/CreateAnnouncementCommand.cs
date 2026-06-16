namespace BaridikExpress.Application.Features.Announcements.Commands.CreateAnnouncement;

public record CreateAnnouncementCommand(
    string TitleAr,
    string TitleEn,
    string TextColor,
    string? DescriptionEn,
    string? DescriptionAr,
    string? Discount,
    string BackgroundColor) : IRequest<Result<Guid>>;