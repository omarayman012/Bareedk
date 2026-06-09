using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.UpdateDeliveryType;

public sealed class UpdateDeliveryTypeCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateDeliveryTypeCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        UpdateDeliveryTypeCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch DeliveryType

        var deliveryType = await db.DeliveryTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (deliveryType is null)
            return Result<bool>.Failure(localizer["DeliveryTypeNotFound"], 404);

        #endregion

        #region Trim (if sent)

        var nameEn = request.NameEn?.Trim();
        var nameAr = request.NameAr?.Trim();
        var descriptionEn = request.DescriptionEn?.Trim();
        var descriptionAr = request.DescriptionAr?.Trim();

        #endregion

        #region Validate Uniqueness (if name sent)

        if (nameEn is not null || nameAr is not null)
        {
            var nameExists = await db.DeliveryTypes
                .AnyAsync(x =>
                    x.Id != request.Id &&
                    (x.NameEn == nameEn || x.NameAr == nameAr),
                    cancellationToken);

            if (nameExists)
                return Result<bool>.Failure(localizer["DeliveryTypeAlreadyExists"]);
        }

        #endregion

        #region Upload Image (if sent)

        var imageUrl = deliveryType.ImageUrl;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "delivery-type-images");

            if (imageUrl is null)
                return Result<bool>.Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Update & Save

        deliveryType.Update(
            nameEn,
            nameAr,
            request.DaysFrom,
            request.DaysTo,
            request.PricePerShipment,
            request.IsSwitchActive,
            request.Currency,
            imageUrl,
            descriptionEn,
            descriptionAr);

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["DeliveryTypeUpdatedSuccessfully"]);
    }

    #endregion
}