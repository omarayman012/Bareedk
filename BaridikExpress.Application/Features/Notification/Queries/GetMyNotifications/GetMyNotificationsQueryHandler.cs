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
        var userId = httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<PaginatedList<MyNotificationResponse>>
                .Failure(localizer["Unauthorized"], 401);
        }

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var query = db.NotificationRecipients
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        if (request.IsRead.HasValue)
        {
            query = query.Where(x => x.IsRead == request.IsRead.Value);
        }

        var projectedQuery = query
            .OrderByDescending(x => x.Notification.CreatedAt)
            .Select(x => new MyNotificationResponse(
                x.Notification.Id,
                new LocalizedDto
                {
                    AR = x.Notification.TitleAr,
                    EN = x.Notification.TitleEn
                },
                new LocalizedDto
                {
                    AR = x.Notification.DescriptionAr,
                    EN = x.Notification.DescriptionEn
                },
                x.Notification.ImageUrl,
                x.IsRead,
                x.ReadAt,
                x.Notification.CreatedAt
            ));

        var result = await PaginatedList<MyNotificationResponse>
            .CreateAsync(projectedQuery, pageNumber, pageSize);

        return Result<PaginatedList<MyNotificationResponse>>
            .Success(result, localizer["NotificationsRetrievedSuccessfully"]);
    }
}