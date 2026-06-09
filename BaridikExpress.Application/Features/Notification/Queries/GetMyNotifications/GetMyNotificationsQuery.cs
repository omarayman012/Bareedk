using BaridikExpress.Application.Features.Notification.DTOs;

namespace BaridikExpress.Application.Features.Notification.Queries.GetMyNotifications;

public sealed record GetMyNotificationsQuery(
    bool? IsRead = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedList<MyNotificationResponse>>>;