using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Notification.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Notification.Queries.GetNotificationById;

public sealed class GetNotificationByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDetailsResponse>>
{
    public async Task<Result<NotificationDetailsResponse>> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        #region Fetch Notification

        var notification = await db.SendNotifications
            .AsNoTracking()
            .Include(x => x.Recipients)
                .ThenInclude(r => r.User)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (notification is null)
            return Result<NotificationDetailsResponse>
                .Failure(localizer["NotificationNotFound"], 404);

        #endregion

        #region Fetch Recipients Data

        var userIds = notification.Recipients
            .Select(r => r.UserId)
            .ToHashSet();

        var clientsCreatedByAdmin = await db.Clients
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById != null)
            .Select(x => new RecipientSummary(x.Id, x.User.FullName))
            .ToListAsync(cancellationToken);

        var clientsExternal = await db.Clients
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById == null)
            .Select(x => new RecipientSummary(x.Id, x.User.FullName))
            .ToListAsync(cancellationToken);

        var deliveriesCreatedByAdmin = await db.Deliveries
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById != null)
            .Select(x => new RecipientSummary(x.Id, x.User.FullName))
            .ToListAsync(cancellationToken);

        var deliveriesExternal = await db.Deliveries
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById == null)
            .Select(x => new RecipientSummary(x.Id, x.User.FullName))
            .ToListAsync(cancellationToken);

        #endregion

        #region Build Response

        var response = new NotificationDetailsResponse(
            notification.Id,
            new LocalizedDto { AR = notification.TitleAr, EN = notification.TitleEn },
            new LocalizedDto { AR = notification.DescriptionAr, EN = notification.DescriptionEn },
            notification.ImageUrl,
            clientsCreatedByAdmin,
            clientsExternal,
            deliveriesCreatedByAdmin,
            deliveriesExternal,
            notification.CreatedBy?.FullName,
            notification.CreatedAt,
            notification.UpdatedBy?.FullName,
            notification.UpdatedAt);

        #endregion

        return Result<NotificationDetailsResponse>
            .Success(response, localizer["NotificationRetrievedSuccessfully"]);
    }
}