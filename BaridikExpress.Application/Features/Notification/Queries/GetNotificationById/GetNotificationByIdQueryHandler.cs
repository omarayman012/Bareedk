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
        var notification = await db.SendNotifications
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new
            {
                x.Id,
                x.TitleAr,
                x.TitleEn,
                x.DescriptionAr,
                x.DescriptionEn,
                x.ImageUrl,
                CreatedByName = x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                UpdatedByName = x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (notification is null)
        {
            return Result<NotificationDetailsResponse>
                .Failure(localizer["NotificationNotFound"], 404);
        }

        var userIds = await db.NotificationRecipients
            .AsNoTracking()
            .Where(x => x.NotificationId == request.Id)
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var clientsCreatedByAdmin = await db.Clients
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById != null)
            .Select(x => new RecipientSummary(
                x.Id,
                x.User.FullName))
            .ToListAsync(cancellationToken);

        var clientsExternal = await db.Clients
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById == null)
            .Select(x => new RecipientSummary(
                x.Id,
                x.User.FullName))
            .ToListAsync(cancellationToken);

        var deliveriesCreatedByAdmin = await db.Deliveries
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById != null)
            .Select(x => new RecipientSummary(
                x.Id,
                x.User.FullName))
            .ToListAsync(cancellationToken);

        var deliveriesExternal = await db.Deliveries
            .AsNoTracking()
            .Where(x => userIds.Contains(x.UserId) && x.CreatedById == null)
            .Select(x => new RecipientSummary(
                x.Id,
                x.User.FullName))
            .ToListAsync(cancellationToken);

        var response = new NotificationDetailsResponse(
            notification.Id,
            new LocalizedDto
            {
                AR = notification.TitleAr,
                EN = notification.TitleEn
            },
            new LocalizedDto
            {
                AR = notification.DescriptionAr,
                EN = notification.DescriptionEn
            },
            notification.ImageUrl,
            clientsCreatedByAdmin,
            clientsExternal,
            deliveriesCreatedByAdmin,
            deliveriesExternal,
            notification.CreatedByName,
            notification.CreatedAt,
            notification.UpdatedByName,
            notification.UpdatedAt);

        return Result<NotificationDetailsResponse>
            .Success(response, localizer["NotificationRetrievedSuccessfully"]);
    }
}