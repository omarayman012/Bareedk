using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Announcements.Commands.UpdateAnnouncement;

public record UpdateAnnouncementCommand(
    Guid Id,
    string? TitleAr,
    string? TitleEn,
    string? TextColor,
    string? DescriptionEn,
    string? DescriptionAr,
        string? Discount,
    string? BackgroundColor) : IRequest<Result<bool>>;
