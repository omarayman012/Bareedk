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
        var query = db.SendNotifications
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var (searchAr, searchEn) = NormalizeHelper.Normalize(
                request.Name,
                request.Name);

            query = query.Where(x =>
                x.TitleAr.Contains(searchAr) ||
                x.TitleEn.Contains(searchEn) ||
                x.DescriptionAr.Contains(searchAr) ||
                x.DescriptionEn.Contains(searchEn));
        }

        var projectedQuery = query
            .OrderByDescending(x => x.CreatedAt)
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
            });

        var totalCount = await projectedQuery.CountAsync(cancellationToken);

        var pageItems = await projectedQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var notificationIds = pageItems
            .Select(x => x.Id)
            .ToList();

        var recipients = await db.NotificationRecipients
            .AsNoTracking()
            .Where(r => notificationIds.Contains(r.NotificationId))
            .Select(r => new
            {
                r.NotificationId,
                r.UserId
            })
            .ToListAsync(cancellationToken);

        var userIds = recipients
            .Select(r => r.UserId)
            .Distinct()
            .ToList();

        // Internal Clients = Customers table
        var customers = await db.Customers
            .AsNoTracking()
            .Where(c => userIds.Contains(c.UserId))
            .Select(c => new
            {
                c.UserId
            })
            .ToListAsync(cancellationToken);

        // External Clients = Clients table
        var clients = await db.Clients
            .AsNoTracking()
            .Where(c => userIds.Contains(c.UserId))
            .Select(c => new
            {
                c.UserId
            })
            .ToListAsync(cancellationToken);

        var deliveries = await db.Deliveries
            .AsNoTracking()
            .Where(d => userIds.Contains(d.UserId))
            .Select(d => new
            {
                d.UserId,
                d.CreatedById
            })
            .ToListAsync(cancellationToken);

        var customerUserIds = customers
            .Select(c => c.UserId)
            .ToHashSet();

        var clientUserIds = clients
            .Select(c => c.UserId)
            .ToHashSet();

        var deliveriesByUserId = deliveries
            .GroupBy(d => d.UserId)
            .ToDictionary(g => g.Key, g => g.First());

        var items = pageItems.Select(n =>
        {
            var notificationRecipients = recipients
                .Where(r => r.NotificationId == n.Id)
                .ToList();

            var recipientUserIds = notificationRecipients
                .Select(r => r.UserId)
                .Distinct()
                .ToList();

            var internalClientsCount = recipientUserIds
                .Count(userId => customerUserIds.Contains(userId));

            var externalClientsCount = recipientUserIds
                .Count(userId => clientUserIds.Contains(userId));

            var totalClientsCount = internalClientsCount + externalClientsCount;

            var notificationDeliveries = recipientUserIds
                .Where(userId => deliveriesByUserId.ContainsKey(userId))
                .Select(userId => deliveriesByUserId[userId])
                .ToList();

            return new NotificationResponse(
                n.Id,
                new LocalizedDto
                {
                    AR = n.TitleAr,
                    EN = n.TitleEn
                },
                new LocalizedDto
                {
                    AR = n.DescriptionAr,
                    EN = n.DescriptionEn
                },
                n.ImageUrl,

                notificationRecipients.Count,

                totalClientsCount,

                // Internal clients from Customers table
                internalClientsCount,

                // External clients from Clients table
                externalClientsCount,

                notificationDeliveries.Count,

                // Internal deliveries
                notificationDeliveries.Count(d => d.CreatedById != null),

                // External deliveries
                notificationDeliveries.Count(d => d.CreatedById == null),

                n.CreatedByName,
                n.CreatedAt,
                n.UpdatedByName,
                n.UpdatedAt
            );
        }).ToList();

        var result = new PaginatedList<NotificationResponse>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedList<NotificationResponse>>
            .Success(result, localizer["NotificationsRetrievedSuccessfully"]);
    }
}