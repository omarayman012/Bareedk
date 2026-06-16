using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.ExportVillages;

public sealed record ExportVillagesQuery : IRequest<Result<byte[]>>;

public sealed class ExportVillagesQueryHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ExportVillagesQueryHandler> localizer)
    : IRequestHandler<ExportVillagesQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        ExportVillagesQuery request,
        CancellationToken cancellationToken)
    {
        var data = await context.Villages
            .AsNoTracking()
            .OrderBy(x => x.VillageNameEn)
            .ToListAsync(cancellationToken);

        var bytes = await excelService.DownloadDataAsync(data);
        return Result<byte[]>.Success(bytes, localizer["ExportedSuccessfully"]);
    }
}