using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.ToggleAnnouncementStatus;

public class ToggleAnnouncementStatusCommandHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer
) : IRequestHandler<
        ToggleAnnouncementStatusCommand,
        Result<bool>
    >
{
    private readonly IGenericRepository<Announcement > _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        ToggleAnnouncementStatusCommand request,
        CancellationToken cancellationToken)
    {
        var announcement = await _repo.GetByIdAsync(request.Id);

        if (announcement is null)
            return Result<bool>.Failure(
                _localizer["AnnouncementNotFound"],
                404 );

        announcement.ToggleStatus();
        await _repo.UpdateAsync(announcement);
        return Result<bool>.Success(
           true,
            _localizer["OperationCompletedSuccessfully"]
        );
    }
}