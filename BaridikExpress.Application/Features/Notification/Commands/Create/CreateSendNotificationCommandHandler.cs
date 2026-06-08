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
        #region Upload Image

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "notification-images");

            if (imageUrl is null)
                return Result<bool>.Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Collect UserIds

        var userIds = new HashSet<string>();

        if (request.ClientsCreatedByAdmin.Count > 0)
        {
            var ids = await db.Customers
                .Where(x => request.ClientsCreatedByAdmin.Contains(x.Id))
                .Select(x => x.UserId)
                .ToListAsync(cancellationToken);
            ids.ForEach(id => userIds.Add(id));
        }

        if (request.DeliveriesCreatedByAdmin.Count > 0)
        {
            var ids = await db.Deliveries
                .Where(x => request.DeliveriesCreatedByAdmin.Contains(x.Id))
                .Select(x => x.UserId)
                .ToListAsync(cancellationToken);
            ids.ForEach(id => userIds.Add(id));
        }

        if (request.ClientsExternalRegistration.Count > 0)
        {
            var ids = await db.Clients
                .Where(x => request.ClientsExternalRegistration.Contains(x.Id))
                .Select(x => x.UserId)
                .ToListAsync(cancellationToken);
            ids.ForEach(id => userIds.Add(id));
        }

        if (request.DeliveriesExternalRegistration.Count > 0)
        {
            var ids = await db.Deliveries
                .Where(x => request.DeliveriesExternalRegistration.Contains(x.Id))
                .Select(x => x.UserId)
                .ToListAsync(cancellationToken);
            ids.ForEach(id => userIds.Add(id));
        }

        if (userIds.Count == 0)
            return Result<bool>.Failure(localizer["NoRecipientsFound"], 404);

        #endregion

        #region Create & Save Notification

        var notification = SendNotification.Create(
            titleAr: request.TitleAr,
            titleEn: request.TitleEn,
            descriptionAr: request.DescriptionAr,
            descriptionEn: request.DescriptionEn,
            userIds: userIds.ToList(),
            imageUrl: imageUrl);

        await db.SendNotifications.AddAsync(notification, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["NotificationSentSuccessfully"]);
    }
}