using BaridikExpress.Application.Features.Notification.DTOs;

namespace BaridikExpress.Application.Features.Notification.Queries.GetAllNotifications;

public sealed record GetAllNotificationsQuery(
    string? Search,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedList<NotificationResponse>>>;