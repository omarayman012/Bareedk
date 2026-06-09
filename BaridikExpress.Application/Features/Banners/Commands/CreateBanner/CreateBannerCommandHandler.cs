using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Commands.CreateBanner;

public sealed class CreateBannerCommandHandler(
    IGenericRepository<Banner> repo,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateBannerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateBannerCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Image is Required

        if (request.Image == null || request.Image.Length == 0)
            return Result<Guid>.Failure(localizer["ImageRequired"], 400);

        #endregion

        #region Normalize Titles & Descriptions

        var (titleAr, titleEn) = NormalizeHelper.Normalize(request.TitleAr, request.TitleEn);
        var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(request.DescriptionAr, request.DescriptionEn);

        #endregion

        #region Validate Uniqueness

        var titleExists = await repo.AnyAsync(x =>
            x.TitleEn == titleEn ||
            x.TitleAr == titleAr,
            cancellationToken);

        if (titleExists)
            return Result<Guid>.Failure(localizer["BannerTitleAlreadyExists"], 409);

        #endregion

        #region Upload Image
        var imagePath = string.Empty;
        if (request.Image is not null)
        {
            using var stream = request.Image.OpenReadStream();
            imagePath = await fileStorage.SaveFileAsync(stream, request.Image.FileName, "banners-images");
        }

        if (string.IsNullOrWhiteSpace(imagePath))
            return Result<Guid>.Failure(localizer["ImageUploadFailed"], 400);

        #endregion

        #region Create & Save

        var banner = new Banner(
            titleEn,
            titleAr,
            descriptionEn,
            descriptionAr,
            imagePath);

        await repo.AddAsync(banner, cancellationToken);

        #endregion

        return Result<Guid>.Success(
            banner.Id,
            localizer["BannerCreatedSuccessfully"],
            201);
    }
}
