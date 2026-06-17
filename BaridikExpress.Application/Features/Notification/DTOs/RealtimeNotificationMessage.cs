namespace BaridikExpress.Application.Features.Notification.DTOs;

public sealed record RealtimeNotificationMessage(
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    string? ImageUrl);