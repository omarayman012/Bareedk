using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Statistics.DTO;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;
using BaridikExpress.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Statistics.Queries.GetStatistics;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, Result<StatisticsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public GetStatisticsQueryHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<StatisticsDto>> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        DateTime? from = request.Filter switch
        {
            StatisticsFilter.Today => now.Date,
            StatisticsFilter.ThisWeek => now.AddDays(-7),
            StatisticsFilter.ThisMonth => new DateTime(now.Year, now.Month, 1),
            StatisticsFilter.Last3Months => now.AddMonths(-3),
            StatisticsFilter.Last6Months => now.AddMonths(-6),
            StatisticsFilter.Last9Months => now.AddMonths(-9),
            StatisticsFilter.ThisYear => new DateTime(now.Year, 1, 1),
            _ => null
        };

        var data = new StatisticsDto
        {
            CareerFields = await Count(_context.CareerFields, from, cancellationToken),
           // OurPlans = await Count(_context.Plans, from, cancellationToken),
            Vehicles = await Count(_context.Vehicles, from, cancellationToken),
            DeliveryTypes = await Count(_context.DeliveryTypes, from, cancellationToken),
            Services = await Count(_context.Services, from, cancellationToken),

            AddedDrivers = await _context.Deliveries
                             .CountAsync(x => x.CreateType == DeliveryCreationType.Internal
                                 && (from == null || x.CreatedAt >= from), cancellationToken),

            ExternalDrivers = await _context.Deliveries
                             .CountAsync(x => x.CreateType == DeliveryCreationType.External
                                 && (from == null || x.CreatedAt >= from), cancellationToken),

            AddedShipments = await Count(_context.Shipments, from, cancellationToken),

            AddedCustomers = await _context.Customers
                             .CountAsync(x => x.CreatedById != null
                                 && (from == null || x.CreatedAt >= from), cancellationToken),

            ExternalCustomers = await _context.Customers
                             .CountAsync(x => x.CreatedById == null
                                 && (from == null || x.CreatedAt >= from), cancellationToken),

            SubAdminsEmployees = await Count(_context.SubAdminEmployees, from, cancellationToken),
            FAQs = await Count(_context.FAQs, from, cancellationToken),
            SendNotifications = await Count(_context.SendNotifications, from, cancellationToken),
            Banners = await Count(_context.Banners, from, cancellationToken),
            BlogsCategories = await Count(_context.BlogsCategorys, from, cancellationToken),
            BlogsAuthors = await Count(_context.BlogsAuthors, from, cancellationToken),
            Blogs = await Count(_context.Blogs, from, cancellationToken),
            PublishingHouses = await Count(_context.PublishingHouses, from, cancellationToken),
            Countries = await Count(_context.Countries, from, cancellationToken),
            Governments = await Count(_context.Governments, from, cancellationToken),
            Cities = await Count(_context.Cities, from, cancellationToken),
            Villages = await Count(_context.Villages, from, cancellationToken),
            Currencies = await Count(_context.Currencies, from, cancellationToken),
            Clients = await Count(_context.Clients, from, cancellationToken),
            Nationalities = await Count(_context.Nationalities, from, cancellationToken),
        };

        return Result<StatisticsDto>.Success(data, _localizer["StatisticsRetrievedSuccessfully"]);
    }

    private static Task<int> Count<T>(
        IQueryable<T> dbSet,
        DateTime? from,
        CancellationToken cancellationToken) where T : class
    {
        if (from is null)
            return dbSet.CountAsync(cancellationToken);

        if (typeof(IAuditableEntity).IsAssignableFrom(typeof(T)))
            return dbSet
                .Cast<IAuditableEntity>()
                .CountAsync(x => x.CreatedAt >= from, cancellationToken);

        return dbSet.CountAsync(cancellationToken);
    }
}