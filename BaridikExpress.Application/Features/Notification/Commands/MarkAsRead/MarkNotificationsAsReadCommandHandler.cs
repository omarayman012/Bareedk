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

        if (string.IsNullOrEmpty(userId))
            return Result<bool>.Failure(localizer["Unauthorized"], 401);

        #endregion

        #region Fetch Recipients

        var recipients = await db.NotificationRecipients
            .Where(x =>
                x.UserId == userId &&
                request.NotificationIds.Contains(x.NotificationId) &&
                !x.IsRead)
            .ToListAsync(cancellationToken);

        if (recipients.Count == 0)
            return Result<bool>.Success(true, localizer["AlreadyMarkedAsRead"]);

        #endregion

        #region Mark As Read & Save

        foreach (var recipient in recipients)
            recipient.MarkAsRead();

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["NotificationsMarkedAsReadSuccessfully"]);
    }
}