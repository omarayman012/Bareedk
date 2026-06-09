namespace BaridikExpress.Application.Features.Notification.Commands.Delete;

public sealed record DeleteNotificationsCommand(
    List<Guid> Ids
) : IRequest<Result<bool>>;