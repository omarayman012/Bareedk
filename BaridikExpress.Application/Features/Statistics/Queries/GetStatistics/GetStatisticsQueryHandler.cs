using BaridikExpress.Application.Features.Statistics.DTO;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Statistics.Queries.GetStatistics;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDto>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsDto> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        return new StatisticsDto
        {
            CareerFields = await _context.CareerFields.CountAsync(cancellationToken),
            Vehicles = await _context.Vehicles.CountAsync(cancellationToken),
            Services = await _context.Services.CountAsync(cancellationToken),
            DeliveryTypes = await _context.DeliveryTypes.CountAsync(cancellationToken),
            Banners = await _context.Banners.CountAsync(cancellationToken),
            Countries = await _context.Countries.CountAsync(cancellationToken),
            Governments = await _context.Governments.CountAsync(cancellationToken),
            Cities = await _context.Cities.CountAsync(cancellationToken),
            Villages = await _context.Villages.CountAsync(cancellationToken),
            Customers = await _context.Customers.CountAsync(cancellationToken),
            Deliveries = await _context.Deliveries.CountAsync(cancellationToken),
            Shipments = await _context.Shipments.CountAsync(cancellationToken),
            Blogs = await _context.Blogs.CountAsync(cancellationToken),
            BlogsCategories = await _context.BlogsCategorys.CountAsync(cancellationToken),
            BlogsAuthors = await _context.BlogsAuthors.CountAsync(cancellationToken),
            PublishingHouses = await _context.PublishingHouses.CountAsync(cancellationToken),
            SubAdminEmployees = await _context.SubAdminEmployees.CountAsync(cancellationToken),
            FAQs = await _context.FAQs.CountAsync(cancellationToken),
            Notifications = await _context.Notifications.CountAsync(cancellationToken)
        };
    }
}