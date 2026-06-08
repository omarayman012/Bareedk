using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Notification.DTOs;

public sealed record NotificationResponse(
    Guid Id,
    LocalizedDto Title,
    LocalizedDto Description,
    string? ImageUrl,
    int RecipientsCount,
    DateTime CreatedAt
);