using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.Announcements.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncements;

public sealed class GetAllAnnouncementsQueryHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllAnnouncementsQuery, Result<PaginatedList<GetAllAnnouncementsDto>>>
{
    public async Task<Result<PaginatedList<GetAllAnnouncementsDto>>> Handle(
        GetAllAnnouncementsQuery request,
        CancellationToken cancellationToken)
    {
        var query = repo.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.TitleEn.Contains(request.Name) ||
                x.TitleAr.Contains(request.Name) ||
                x.BackgroundColor.Contains(request.Name) ||
                x.TextColor.Contains(request.Name));

        query = query.ApplyCommonFilters(request);

        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllAnnouncementsDto
            {
                Id = x.Id,
                Title = new LocalizedDto
                {
                    EN = x.TitleEn,
                    AR = x.TitleAr
                },
                Description = new LocalizedDto
                {
                    EN = x.DescriptionEn,
                    AR = x.DescriptionAr
                },
                Discount = x.Discount,
                BackgroundColor = x.BackgroundColor,
                TextColor = x.TextColor,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : "",
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : "",
                UpdatedAt = x.UpdatedAt
            });

        var paginatedResult = await PaginatedList<GetAllAnnouncementsDto>
            .CreateAsync(result, request.PageNumber, request.PageSize);

        return Result<PaginatedList<GetAllAnnouncementsDto>>
            .Success(paginatedResult, localizer["OperationCompletedSuccessfully"]);
    }
}