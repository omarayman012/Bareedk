
using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Queries.ExportServices;

public sealed class ExportServicesQueryHandler(
    IApplicationDbContext context,
    IExcelService excelService)
    : IRequestHandler<ExportServicesQuery, byte[]>
{
    public async Task<byte[]> Handle(
        ExportServicesQuery request,
        CancellationToken cancellationToken)
    {
        var services = await context.Services
            .AsNoTracking()
            .Select(x => new ServiceExcelDto
            {
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                Price = x.Price,
                Currency = x.Currency,
                ImageUrl = x.ImageUrl
            })
            .ToListAsync(cancellationToken);

        return await excelService.DownloadDataAsync(services);
    }
}