using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.ExportCities;

public sealed record ExportCitiesQuery : IRequest<Result<byte[]>>;

public sealed class ExportCitiesQueryHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ExportCitiesQueryHandler> localizer)
    : IRequestHandler<ExportCitiesQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        ExportCitiesQuery request,
        CancellationToken cancellationToken)
    {
        var data = await context.Cities
            .AsNoTracking()
            .OrderBy(x => x.CityNameEn)
            .ToListAsync(cancellationToken);

        var bytes = await excelService.DownloadDataAsync(data);
        return Result<byte[]>.Success(bytes, localizer["ExportedSuccessfully"]);
    }
}