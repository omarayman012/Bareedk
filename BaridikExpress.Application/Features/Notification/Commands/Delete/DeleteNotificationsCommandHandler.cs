using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Commands.Delete;

public sealed class DeleteNotificationsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteNotificationsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteNotificationsCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Notifications

        var notifications = await db.SendNotifications
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(notifications.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeNotificationsNotFound"]);

        #endregion

        #region Delete & Save

        db.SendNotifications.RemoveRange(notifications);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["NotificationsDeletedSuccessfully"]);
    }
}