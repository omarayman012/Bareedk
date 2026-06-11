using BaridikExpress.Application.Features.Announcements.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;
namespace BaridikExpress.Application.Features.Announcements.Queries.GetAnnouncementById
{
    public class GetAnnouncementByIdQueryHandler(
        IGenericRepository<Announcement> repo,
        IStringLocalizer localizer)
        : IRequestHandler<GetAnnouncementByIdQuery, Result<GetAnnouncementByIdDto>>
    {
        private readonly IGenericRepository<Announcement> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<GetAnnouncementByIdDto>> Handle(
            GetAnnouncementByIdQuery request,
            CancellationToken cancellationToken)
        {
            var announcement = await _repo.Query()
        .Include(x => x.CreatedBy)
        .Include(x => x.UpdatedBy)
        .Where(x => x.Id == request.Id)
        .Select(x => new GetAnnouncementByIdDto
        {
            Id = x.Id,
            Title = new LocalizedDto
            {
                EN = x.TitleEn,
                AR = x.TitleAr
            },
            BackgroundColor = x.BackgroundColor,
            TextColor = x.TextColor,
            IsActive = x.IsActive,
            CreatedBy = x.CreatedBy != null
                ? x.CreatedBy.FullName
                : string.Empty,
            CreatedAt = x.CreatedAt,
            UpdatedBy = x.UpdatedBy != null
                ? x.UpdatedBy.FullName
                : string.Empty,

            UpdatedAt = x.UpdatedAt
        })
        .FirstOrDefaultAsync(cancellationToken);

            if (announcement is null)
                return Result<GetAnnouncementByIdDto>
                    .Failure(_localizer["AnnouncementNotFound"], 404);

            return Result<GetAnnouncementByIdDto>
                .Success(
                    announcement,
                    _localizer["OperationCompletedSuccessfully"]);
        }
    }
}

