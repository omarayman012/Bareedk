using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Banners.Commands.UpdateBanner;

public sealed class UpdateBannerCommandHandler(
    IGenericRepository<Banner> repo,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateBannerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateBannerCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Banner

        var banner = await repo.GetByIdAsync(request.Id, cancellationToken);

        if (banner is null)
            return Result<bool>.Failure(localizer["BannerNotFound"], 404);

        #endregion

        #region Prepare Title & Description (keep old values if not provided)

        var titleAr = string.IsNullOrWhiteSpace(request.TitleAr) ? banner.TitleAr : request.TitleAr;
        var titleEn = string.IsNullOrWhiteSpace(request.TitleEn) ? banner.TitleEn : request.TitleEn;
        var descriptionAr = string.IsNullOrWhiteSpace(request.DescriptionAr) ? banner.DescriptionAr : request.DescriptionAr;
        var descriptionEn = string.IsNullOrWhiteSpace(request.DescriptionEn) ? banner.DescriptionEn : request.DescriptionEn;

        #endregion

        #region Normalize Titles & Descriptions

        (titleAr, titleEn) = NormalizeHelper.Normalize(titleAr, titleEn);
        (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(descriptionAr, descriptionEn);

        #endregion

        #region Validate Uniqueness (if title changed)

        if (titleAr != banner.TitleAr || titleEn != banner.TitleEn)
        {
            var titleExists = await repo.Query()
                .Where(x =>
                    x.Id != request.Id &&
                    (x.TitleEn == titleEn ||
                     x.TitleAr == titleAr))
                .AnyAsync(cancellationToken);

            if (titleExists)
                return Result<bool>.Failure(localizer["BannerTitleAlreadyExists"], 409);
        }

        #endregion

        #region Upload Image (if provided)

        string? imagePath = banner.ImageUrl;

        if (request.Image is not null)
        {
            using var stream = request.Image.OpenReadStream();

            imagePath = await fileStorage.UpdateFileAsync(
                stream,
                request.Image.FileName,
                banner.ImageUrl,
                "banners-images");
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return Result<bool>.Failure(
                    localizer["ImageUploadFailed"]);
            }
        }

        #endregion

        #region Update & Save

        banner.Update(titleEn, titleAr, descriptionEn, descriptionAr, imagePath);

        await repo.UpdateAsync(banner, cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["BannerUpdatedSuccessfully"]);
    }
}
