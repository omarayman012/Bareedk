using BaridikExpress.Application.Features.ContactUs.DTOs;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ContactUs.Queries.ExportContactUs;

public sealed class ExportContactUsQueryHandler(
    IApplicationDbContext db,
    IExcelService excelService)
    : IRequestHandler<ExportContactUsQuery, byte[]>
{
    public async Task<byte[]> Handle(
        ExportContactUsQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.ContactUs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x =>
                x.Name.Contains(request.Search) ||
                x.Email.Contains(request.Search) ||
                x.Phone.Contains(request.Search));

        if (request.IsRead.HasValue)
            query = query.Where(x => x.IsRead == request.IsRead.Value);

        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactUsResponse(
                x.Id,
                x.Name,
                x.Email,
                x.Phone,
                x.Message,
                x.IsRead,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        return await excelService.DownloadDataAsync(data);
    }
}