using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.Notification.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Commands.Create;

public sealed class CreateSendNotificationCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage,
    INotificationService notificationService)
    : IRequestHandler<CreateSendNotificationCommand, Result<bool>>
{
    private const int NotificationBatchSize = 500;

    public async Task<Result<bool>> Handle(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var (titleAr, titleEn) = NormalizeHelper.Normalize(
            request.TitleAr,
            request.TitleEn);

        var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(
            request.DescriptionAr,
            request.DescriptionEn);

        var imageUrlResult = await UploadImageAsync(
            request,
            cancellationToken);

        if (!imageUrlResult.IsSuccess)
        {
            return Result<bool>.Failure(
                imageUrlResult.Message!,
                imageUrlResult.StatusCode);
        }

        var userIds = await CollectUserIdsAsync(
            request,
            cancellationToken);

        if (userIds.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["NoRecipientsFound"],
                404);
        }

        var sendNotification = SendNotification.Create(
            titleAr: titleAr,
            titleEn: titleEn,
            descriptionAr: descriptionAr,
            descriptionEn: descriptionEn,
            userIds: userIds,
            imageUrl: imageUrlResult.Data);

        await db.SendNotifications.AddAsync(
            sendNotification,
            cancellationToken);

        var userNotifications = userIds
            .Select(userId => Domain.Entities.NotificationModules.Notification.Create(
                userId: userId,
                titleAr: titleAr,
                titleEn: titleEn,
                messageAr: descriptionAr,
                messageEn: descriptionEn,
                imageUrl: imageUrlResult.Data,
                sendNotificationId: sendNotification.Id))
            .ToList();

        await db.Notifications.AddRangeAsync(
            userNotifications,
            cancellationToken);

        await db.SaveChangesAsync(cancellationToken);

        var message = new RealtimeNotificationMessage(
            TitleAr: titleAr,
            TitleEn: titleEn,
            DescriptionAr: descriptionAr,
            DescriptionEn: descriptionEn,
            ImageUrl: imageUrlResult.Data);

        foreach (var userIdsBatch in userIds.Chunk(NotificationBatchSize))
        {
            await notificationService.SendAsync(
                userIdsBatch.ToList(),
                message,
                cancellationToken);
        }

        return Result<bool>.Success(
            true,
            localizer["NotificationSentSuccessfully"]);
    }

    private async Task<Result<string?>> UploadImageAsync(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null)
        {
            return Result<string?>.Success(null);
        }

        var extension = Path.GetExtension(request.Image.FileName);

        if (string.IsNullOrWhiteSpace(extension))
        {
            return Result<string?>.Failure(
                localizer["InvalidImageExtension"],
                400);
        }

        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";

        await using var stream = request.Image.OpenReadStream();

        var imageUrl = await fileStorage.SaveFileAsync(
            stream,
            uniqueFileName,
            "notification-images");

        if (imageUrl is null)
        {
            return Result<string?>.Failure(
                localizer["ImageUploadFailed"],
                400);
        }

        return Result<string?>.Success(imageUrl);
    }

    private async Task<List<string>> CollectUserIdsAsync(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var userIds = new HashSet<string>();

        if (request.ClientsCreatedByAdmin.Count > 0)
        {
            var ids = await db.Customers
                .AsNoTracking()
                .Where(x => request.ClientsCreatedByAdmin.Contains(x.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                .Select(x => x.UserId!)
                .ToListAsync(cancellationToken);

            userIds.UnionWith(ids);
        }

        if (request.DeliveriesCreatedByAdmin.Count > 0)
        {
            var ids = await db.Deliveries
                .AsNoTracking()
                .Where(x => request.DeliveriesCreatedByAdmin.Contains(x.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                .Select(x => x.UserId!)
                .ToListAsync(cancellationToken);

            userIds.UnionWith(ids);
        }

        if (request.ClientsExternalRegistration.Count > 0)
        {
            var ids = await db.Clients
                .AsNoTracking()
                .Where(x => request.ClientsExternalRegistration.Contains(x.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                .Select(x => x.UserId!)
                .ToListAsync(cancellationToken);

            userIds.UnionWith(ids);
        }

        if (request.DeliveriesExternalRegistration.Count > 0)
        {
            var ids = await db.Deliveries
                .AsNoTracking()
                .Where(x => request.DeliveriesExternalRegistration.Contains(x.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                .Select(x => x.UserId!)
                .ToListAsync(cancellationToken);

            userIds.UnionWith(ids);
        }

        return userIds.ToList();
    }
}