using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Commands.Create;

public sealed class CreateSendNotificationCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateSendNotificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var imageUrlResult = await UploadImageAsync(request, cancellationToken);

        if (!imageUrlResult.IsSuccess)
            return Result<bool>.Failure(imageUrlResult.Message!, imageUrlResult.StatusCode);

        var userIds = await CollectUserIdsAsync(request, cancellationToken);

        if (userIds.Count == 0)
            return Result<bool>.Failure(localizer["NoRecipientsFound"], 404);

        var notification = SendNotification.Create(
            titleAr: request.TitleAr,
            titleEn: request.TitleEn,
            descriptionAr: request.DescriptionAr,
            descriptionEn: request.DescriptionEn,
            userIds: userIds,
            imageUrl: imageUrlResult.ToString());

        await db.SendNotifications.AddAsync(notification, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["NotificationSentSuccessfully"]);
    }

    private async Task<Result<string?>> UploadImageAsync(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null)
            return Result<string?>.Success(null);

        var extension = Path.GetExtension(request.Image.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";

        await using var stream = request.Image.OpenReadStream();

        var imageUrl = await fileStorage.SaveFileAsync(
            stream,
            uniqueFileName,
            "notification-images");

        if (imageUrl is null)
            return Result<string?>.Failure(localizer["ImageUploadFailed"], 400);

        return Result<string?>.Success(imageUrl);
    }

    private async Task<List<string>> CollectUserIdsAsync(
        CreateSendNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<List<string>>>();

        if (request.ClientsCreatedByAdmin.Count > 0)
        {
            tasks.Add(
                db.Customers
                    .AsNoTracking()
                    .Where(x => request.ClientsCreatedByAdmin.Contains(x.Id))
                    .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                    .Select(x => x.UserId!)
                    .ToListAsync(cancellationToken));
        }

        if (request.DeliveriesCreatedByAdmin.Count > 0)
        {
            tasks.Add(
                db.Deliveries
                    .AsNoTracking()
                    .Where(x => request.DeliveriesCreatedByAdmin.Contains(x.Id))
                    .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                    .Select(x => x.UserId!)
                    .ToListAsync(cancellationToken));
        }

        if (request.ClientsExternalRegistration.Count > 0)
        {
            tasks.Add(
                db.Clients
                    .AsNoTracking()
                    .Where(x => request.ClientsExternalRegistration.Contains(x.Id))
                    .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                    .Select(x => x.UserId!)
                    .ToListAsync(cancellationToken));
        }

        if (request.DeliveriesExternalRegistration.Count > 0)
        {
            tasks.Add(
                db.Deliveries
                    .AsNoTracking()
                    .Where(x => request.DeliveriesExternalRegistration.Contains(x.Id))
                    .Where(x => !string.IsNullOrWhiteSpace(x.UserId))
                    .Select(x => x.UserId!)
                    .ToListAsync(cancellationToken));
        }

        if (tasks.Count == 0)
            return [];

        var results = await Task.WhenAll(tasks);

        return results
            .SelectMany(x => x)
            .Distinct()
            .ToList();
    }
}