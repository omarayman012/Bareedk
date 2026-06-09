using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;
using Microsoft.Extensions.Logging;

namespace BaridikExpress.Application.Features.Banners.Commands.DeleteBanner;

public sealed class DeleteBannerCommandHandler(
    IGenericRepository<Banner> repo,
    IFileStorageService fileStorageService,
    IStringLocalizer localizer,
    ILogger<DeleteBannerCommandHandler> logger)
    : IRequestHandler<DeleteBannerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteBannerCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Input

        if (request.Ids is null || !request.Ids.Any())
            return Result<bool>.Failure(
                localizer["InvalidIds"],
                400);

        #endregion

        #region Fetch Banners

        var banners = await repo.Query()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!banners.Any())
            return Result<bool>.Failure(
                localizer["BannersNotFound"],
                404);

        if (banners.Count != request.Ids.Count)
            return Result<bool>.Failure(
                localizer["SomeBannersNotFound"],
                404);

        #endregion

        #region Store Image Paths

        var imagePaths = banners
            .Where(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .Select(x => x.ImageUrl)
            .ToList();

        #endregion

        #region Delete Banners

        await repo.DeleteRangeAsync(banners);

        #endregion

        #region Delete Images

        foreach (var imagePath in imagePaths)
        {
            try
            {
                await fileStorageService.DeleteFileAsync(imagePath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Failed to delete banner image {ImagePath}",
                    imagePath);
            }
        }

        #endregion

        return Result<bool>.Success(
            true,
            localizer["BannersDeletedSuccessfully"]);
    }
}