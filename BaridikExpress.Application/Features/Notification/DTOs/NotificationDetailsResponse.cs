using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Notification.DTOs;

public sealed record NotificationDetailsResponse(
    Guid Id,
    LocalizedDto Title,
    LocalizedDto Description,
    string? ImageUrl,
    IReadOnlyList<RecipientSummary> ClientsCreatedByAdmin,
    IReadOnlyList<RecipientSummary> ClientsExternalRegistration,
    IReadOnlyList<RecipientSummary> DeliveriesCreatedByAdmin,
    IReadOnlyList<RecipientSummary> DeliveriesExternalRegistration,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);

public sealed record RecipientSummary(
    Guid Id,
    string FullName
);