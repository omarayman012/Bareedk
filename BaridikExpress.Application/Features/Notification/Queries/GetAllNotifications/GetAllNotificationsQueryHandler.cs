using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Notification.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Queries.GetAllNotifications;

public sealed class GetAllNotificationsQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllNotificationsQuery, Result<PaginatedList<NotificationResponse>>>
{
    public async Task<Result<PaginatedList<NotificationResponse>>> Handle(
        GetAllNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        #region Build Query

        var query = db.SendNotifications.AsNoTracking().AsQueryable();

        #endregion

        #region Filters

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var (searchAr, searchEn) = NormalizeHelper.Normalize(request.Search, request.Search);

            query = query.Where(x =>
                x.TitleAr.Contains(searchAr) ||
                x.TitleEn.Contains(searchEn) ||
                x.DescriptionAr.Contains(searchAr) ||
                x.DescriptionEn.Contains(searchEn));
        }

        #endregion

        #region Projection

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NotificationResponse(
                x.Id,
                new LocalizedDto { AR = x.TitleAr, EN = x.TitleEn },
                new LocalizedDto { AR = x.DescriptionAr, EN = x.DescriptionEn },
                x.ImageUrl,
                x.Recipients.Count,
                x.CreatedAt));

        #endregion

        #region Paginate

        var result = await PaginatedList<NotificationResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        #endregion

        return Result<PaginatedList<NotificationResponse>>
            .Success(result, localizer["NotificationsRetrievedSuccessfully"]);
    }
}