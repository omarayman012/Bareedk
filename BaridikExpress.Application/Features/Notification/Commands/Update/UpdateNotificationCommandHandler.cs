using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Commands.Update;

public sealed class UpdateNotificationCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateNotificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Notification

        var notification = await db.SendNotifications
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (notification is null)
            return Result<bool>.Failure(localizer["NotificationNotFound"], 404);

        #endregion

        #region Upload Image (if sent)

        var imageUrl = notification.ImageUrl;

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

        #region Update & Save

        notification.Update(
            request.TitleAr,
            request.TitleEn,
            request.DescriptionAr,
            request.DescriptionEn,
            imageUrl);

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["NotificationUpdatedSuccessfully"]);
    }
}