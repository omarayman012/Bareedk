using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.ExportGovernments;

public sealed record ExportGovernmentsQuery : IRequest<Result<byte[]>>;

public sealed class ExportGovernmentsQueryHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ExportGovernmentsQueryHandler> localizer)
    : IRequestHandler<ExportGovernmentsQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        ExportGovernmentsQuery request,
        CancellationToken cancellationToken)
    {
        var data = await context.Governments
            .AsNoTracking()
            .OrderBy(x => x.GovernmentNameEn)
            .ToListAsync(cancellationToken);

        var bytes = await excelService.DownloadDataAsync(data);
        return Result<byte[]>.Success(bytes, localizer["ExportedSuccessfully"]);
    }
}