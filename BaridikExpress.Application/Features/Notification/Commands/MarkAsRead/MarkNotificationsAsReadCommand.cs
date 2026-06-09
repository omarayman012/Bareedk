namespace BaridikExpress.Application.Features.Notification.Commands.MarkAsRead;

public sealed record MarkNotificationsAsReadCommand(
    List<Guid> NotificationIds
) : IRequest<Result<bool>>;