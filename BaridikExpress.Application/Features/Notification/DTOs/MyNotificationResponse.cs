using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Notification.DTOs;

public sealed record MyNotificationResponse(
    Guid Id,
    LocalizedDto Title,
    LocalizedDto Description,
    string? ImageUrl,
    bool IsRead,
    DateTime? ReadAt,
    DateTime CreatedAt
);