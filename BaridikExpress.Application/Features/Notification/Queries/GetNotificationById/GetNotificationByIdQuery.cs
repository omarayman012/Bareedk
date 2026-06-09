using BaridikExpress.Application.Features.Notification.DTOs;

namespace BaridikExpress.Application.Features.Notification.Queries.GetNotificationById;

public sealed record GetNotificationByIdQuery(
    Guid Id
) : IRequest<Result<NotificationDetailsResponse>>;