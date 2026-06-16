using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.Export;

public sealed class ExportServiceBusinessPlansQueryHandler(
IApplicationDbContext db,
IExcelService excelService)
: IRequestHandler<ExportServiceBusinessPlansQuery, byte[]>
{
    public async Task<byte[]> Handle(
    ExportServiceBusinessPlansQuery request,
    CancellationToken cancellationToken)
    {
        var data = await db.ServiceBusinessPlans
        .AsNoTracking()
        .ToListAsync(cancellationToken);


    return await excelService.DownloadDataAsync(data);
    }

}
