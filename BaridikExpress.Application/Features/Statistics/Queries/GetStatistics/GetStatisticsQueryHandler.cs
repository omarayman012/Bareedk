using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Statistics.DTO;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.Base;
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
            Vehicles = await Count(_context.Vehicles, from, cancellationToken),
            Services = await Count(_context.Services, from, cancellationToken),
            DeliveryTypes = await Count(_context.DeliveryTypes, from, cancellationToken),
            Banners = await Count(_context.Banners, from, cancellationToken),
            Countries = await Count(_context.Countries, from, cancellationToken),
            Governments = await Count(_context.Governments, from, cancellationToken),
            Cities = await Count(_context.Cities, from, cancellationToken),
            Villages = await Count(_context.Villages, from, cancellationToken),
            Customers = await Count(_context.Customers, from, cancellationToken),
            Deliveries = await Count(_context.Deliveries, from, cancellationToken),
            Shipments = await Count(_context.Shipments, from, cancellationToken),
            Blogs = await Count(_context.Blogs, from, cancellationToken),
            BlogsCategories = await Count(_context.BlogsCategorys, from, cancellationToken),
            BlogsAuthors = await Count(_context.BlogsAuthors, from, cancellationToken),
            PublishingHouses = await Count(_context.PublishingHouses, from, cancellationToken),
            SubAdminEmployees = await Count(_context.SubAdminEmployees, from, cancellationToken),
            FAQs = await Count(_context.FAQs, from, cancellationToken),
            Notifications = await Count(_context.Notifications, from, cancellationToken)
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