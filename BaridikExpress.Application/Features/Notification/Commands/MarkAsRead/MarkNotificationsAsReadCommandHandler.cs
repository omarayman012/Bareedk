using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BaridikExpress.Application.Features.Notification.Commands.MarkAsRead;

public sealed class MarkNotificationsAsReadCommandHandler(
    IApplicationDbContext db,
    IHttpContextAccessor httpContextAccessor,
    IStringLocalizer localizer)
    : IRequestHandler<MarkNotificationsAsReadCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        MarkNotificationsAsReadCommand request,
        CancellationToken cancellationToken)
    {
        #region Get Current User

        var userId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<bool>.Failure(
                localizer["Unauthorized"],
                401);
        }

        #endregion

        #region Validate Request

        var notificationIds = request.NotificationIds
            .Distinct()
            .ToList();

        if (notificationIds.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["NotificationIdsRequired"],
                400);
        }

        #endregion

        #region Fetch Notifications

        var notifications = await db.Notifications
            .Where(x =>
                x.UserId == userId &&
                notificationIds.Contains(x.Id) &&
                !x.IsRead)
            .ToListAsync(cancellationToken);

        if (notifications.Count == 0)
        {
            return Result<bool>.Success(
                true,
                localizer["AlreadyMarkedAsRead"]);
        }

        #endregion

        #region Mark As Read & Save

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(
            true,
            localizer["NotificationsMarkedAsReadSuccessfully"]);
    }
}