using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Notification.DTOs;

public sealed record NotificationResponse(
    Guid Id,
    LocalizedDto Title,
    LocalizedDto Description,
    string? ImageUrl,
    int TotalRecipientsCount,
    int ClientsCount,
    int ClientsCreatedByAdminCount,
    int ClientsExternalCount,
    int DeliveriesCount,
    int DeliveriesCreatedByAdminCount,
    int DeliveriesExternalCount,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);