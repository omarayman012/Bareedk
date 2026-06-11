using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.UpdateAnnouncement;

public sealed class UpdateAnnouncementCommandHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateAnnouncementCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateAnnouncementCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Announcement

        var banner = await repo.GetByIdAsync(request.Id, cancellationToken);

        if (banner is null)
            return Result<bool>.Failure(localizer["AnnouncementNotFound"], 404);

        #endregion

        #region Prepare Title & Description (keep old values if not provided)

        var titleAr = string.IsNullOrWhiteSpace(request.TitleAr) ? banner.TitleAr : request.TitleAr;
        var titleEn = string.IsNullOrWhiteSpace(request.TitleEn) ? banner.TitleEn : request.TitleEn;

        #endregion

        #region Normalize Titles

        (titleAr, titleEn) = NormalizeHelper.Normalize(titleAr, titleEn);
        #endregion


        #region Update & Save

        banner.Update(titleEn, titleAr, request.TextColor, request.BackgroundColor);
        await repo.UpdateAsync(banner, cancellationToken);
        #endregion

        return Result<bool>.Success(true, localizer["AnnouncementUpdatedSuccessfully"]);
    }
}
