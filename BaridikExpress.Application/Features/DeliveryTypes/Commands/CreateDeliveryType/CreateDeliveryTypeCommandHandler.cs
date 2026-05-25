using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.DeliveryTypes.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.DeliveryType;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;

public sealed class CreateDeliveryTypeCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateDeliveryTypeCommand, Result<DeliveryTypeResponse>>
{
    #region Handle

    public async Task<Result<DeliveryTypeResponse>> Handle(
        CreateDeliveryTypeCommand request,
        CancellationToken cancellationToken)
    {
        #region Normalize Name & Description

        var (nameAr, nameEn) = NormalizeHelper.Normalize(request.NameAr, request.NameEn);
        var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(request.DescriptionAr, request.DescriptionEn);

        #endregion

        #region Validate Uniqueness

        var nameExists = await db.DeliveryTypes
            .AnyAsync(x =>
                x.NameEn == nameEn ||
                x.NameAr == nameAr,
                cancellationToken);

        if (nameExists)
            return Result<DeliveryTypeResponse>
                .Failure(localizer["DeliveryTypeAlreadyExists"]);

        #endregion

        #region Upload Image

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "delivery-type-images");

            if (imageUrl is null)
                return Result<DeliveryTypeResponse>
                    .Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Create & Save

        var deliveryType = DeliveryType.Create(
            nameEn,
            nameAr,
            request.DaysFrom,
            request.DaysTo,
            request.PricePerShipment,
            request.IsSwitchActive,
            imageUrl,
            descriptionEn,
            descriptionAr);

        await db.DeliveryTypes.AddAsync(deliveryType, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        #region Map Response

        var response = new DeliveryTypeResponse(
            deliveryType.Id,
            new LocalizedDto { EN = deliveryType.NameEn, AR = deliveryType.NameAr },
            deliveryType.DaysFrom,
            deliveryType.DaysTo,
            deliveryType.PricePerShipment,
            deliveryType.PricePerTotal,
            deliveryType.IsSwitchActive,
            deliveryType.ImageUrl,
            new LocalizedDto { EN = deliveryType.DescriptionEn, AR = deliveryType.DescriptionAr },
            deliveryType.CreatedBy?.FullName,
            deliveryType.CreatedAt,
            deliveryType.UpdatedBy?.FullName,
            deliveryType.UpdatedAt);

        #endregion

        return Result<DeliveryTypeResponse>
            .Success(response, localizer["DeliveryTypeCreatedSuccessfully"], 201);
    }

    #endregion
}