using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Notification.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BaridikExpress.Application.Features.Notification.Queries.GetMyNotifications;

public sealed class GetMyNotificationsQueryHandler(
    IApplicationDbContext db,
    IHttpContextAccessor httpContextAccessor,
    IStringLocalizer localizer)
    : IRequestHandler<GetMyNotificationsQuery, Result<PaginatedList<MyNotificationResponse>>>
{
    public async Task<Result<PaginatedList<MyNotificationResponse>>> Handle(
        GetMyNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        #region Get Current User

        var userId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<PaginatedList<MyNotificationResponse>>
                .Failure(localizer["Unauthorized"], 401);
        }

        #endregion

        #region Build Query

        var query = db.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        #endregion

        #region Filters

        if (request.IsRead.HasValue)
        {
            query = query.Where(x => x.IsRead == request.IsRead.Value);
        }

        #endregion

        #region Projection

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MyNotificationResponse(
                x.Id,
                new LocalizedDto
                {
                    AR = x.TitleAr,
                    EN = x.TitleEn
                },
                new LocalizedDto
                {
                    AR = x.MessageAr,
                    EN = x.MessageEn
                },
                x.ImageUrl,
                x.IsRead,
                null,
                x.CreatedAt));

        #endregion

        #region Paginate

        var result = await PaginatedList<MyNotificationResponse>
            .CreateAsync(
                projected,
                request.PageNumber,
                request.PageSize);

        #endregion

        return Result<PaginatedList<MyNotificationResponse>>
            .Success(
                result,
                localizer["NotificationsRetrievedSuccessfully"]);
    }
}