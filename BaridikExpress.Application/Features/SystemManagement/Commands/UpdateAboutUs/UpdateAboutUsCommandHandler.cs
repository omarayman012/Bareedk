using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateAboutUs;

public sealed class UpdateAboutUsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateAboutUsCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        UpdateAboutUsCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch AboutUs

        var aboutUs = await db.AboutUs
            .FirstOrDefaultAsync(cancellationToken);

        if (aboutUs is null)
            return Result<bool>.Failure(localizer["AboutUsNotFound"], 404);

        #endregion

        #region Upload Image (if sent)

        var imageUrl = aboutUs.ImageUrl;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "about-us-images");

            if (imageUrl is null)
                return Result<bool>.Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Upload Video (if sent)

        var videoUrl = aboutUs.VideoUrl;

        if (request.Video is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Video.FileName)}";

            videoUrl = await fileStorage.SaveFileAsync(
                request.Video.OpenReadStream(),
                uniqueFileName,
                "about-us-videos");

            if (videoUrl is null)
                return Result<bool>.Failure(localizer["VideoUploadFailed"], 400);
        }

        #endregion

        #region Update & Save

        aboutUs.Update(
            request.TitleAr,
            request.TitleEn,
            request.DescriptionAr,
            request.DescriptionEn,
            imageUrl,
            videoUrl,
            request.ExternalLinkYoutube);

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["AboutUsUpdatedSuccessfully"]);
    }

    #endregion
}