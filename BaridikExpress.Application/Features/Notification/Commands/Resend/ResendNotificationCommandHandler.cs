using BaridikExpress.Application.Features.Notification.DTOs;
using BaridikExpress.Application.Interfaces.Realtime;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Commands.Resend;

public sealed class ResendNotificationCommandHandler(
    IApplicationDbContext db,
    INotificationService notificationService,
    IStringLocalizer localizer)
    : IRequestHandler<ResendNotificationCommand, Result<bool>>
{
    private const int NotificationBatchSize = 500;

    public async Task<Result<bool>> Handle(
        ResendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await db.SendNotifications
            .AsNoTracking()
            .Where(x => x.Id == request.SendNotificationId)
            .Select(x => new ResendNotificationData(
                x.TitleAr,
                x.TitleEn,
                x.DescriptionAr,
                x.DescriptionEn,
                x.ImageUrl))
            .FirstOrDefaultAsync(cancellationToken);

        if (notification is null)
        {
            return Result<bool>.Failure(
                localizer["NotificationNotFound"],
                404);
        }

        var userIds = await db.NotificationRecipients
            .AsNoTracking()
            .Where(x => x.NotificationId == request.SendNotificationId)
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (userIds.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["NoRecipientsFound"],
                404);
        }

        var message = new RealtimeNotificationMessage(
            notification.TitleAr,
            notification.TitleEn,
            notification.DescriptionAr,
            notification.DescriptionEn,
            notification.ImageUrl);

        foreach (var userIdsBatch in userIds.Chunk(NotificationBatchSize))
        {
            await notificationService.SendAsync(
                userIdsBatch.ToList(),
                message,
                cancellationToken);
        }

        return Result<bool>.Success(
            true,
            localizer["NotificationResentSuccessfully"]);
    }

    private sealed record ResendNotificationData(
        string TitleAr,
        string TitleEn,
        string DescriptionAr,
        string DescriptionEn,
        string? ImageUrl);
}