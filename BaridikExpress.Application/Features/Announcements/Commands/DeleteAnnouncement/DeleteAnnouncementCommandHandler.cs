using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.DeleteAnnouncement;

public sealed class DeleteAnnouncementCommandHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteAnnouncementCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteAnnouncementCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Input

        if (request.Ids is null || !request.Ids.Any())
            return Result<bool>.Failure(
                localizer["InvalidIds"],
                400);

        #endregion

        #region Fetch Announcements

        var banners = await repo.Query()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!banners.Any())
            return Result<bool>.Failure(
                localizer["AnnouncementsNotFound"],
                404);

        if (banners.Count != request.Ids.Count)
            return Result<bool>.Failure(
                localizer["SomeAnnouncementsNotFound"],
                404);

        #endregion

        #region Delete Announcements

        await repo.DeleteRangeAsync(banners);

        #endregion

        return Result<bool>.Success(
            true,
            localizer["AnnouncementsDeletedSuccessfully"]);
    }
}