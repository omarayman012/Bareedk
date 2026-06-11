using BaridikExpress.Application.Features.Announcements.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncementsMobile;

public sealed class GetAllAnnouncementsMobileQueryHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllAnnouncementsMobileQuery, Result<PaginatedList<GetAllAnnouncementsMobileDto>>>
{
    public async Task<Result<PaginatedList<GetAllAnnouncementsMobileDto>>> Handle(
        GetAllAnnouncementsMobileQuery request,
        CancellationToken cancellationToken)
    {
        var query = repo.Query()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllAnnouncementsMobileDto
            {
                Id = x.Id,
                Title = new LocalizedDto
                {
                    EN = x.TitleEn,
                    AR = x.TitleAr
                },
                BackgroundColor = x.BackgroundColor,
                TextColor = x.TextColor,
                IsActive = x.IsActive
            });

        var result = await PaginatedList<GetAllAnnouncementsMobileDto>
            .CreateAsync(
                query,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<GetAllAnnouncementsMobileDto>>
            .Success(
                result,
                localizer["OperationCompletedSuccessfully"]);
    }
}