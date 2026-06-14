using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;


namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.ExportCountries;

public sealed record ExportCountriesQuery : IRequest<Result<byte[]>>;

public sealed class ExportCountriesQueryHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ExportCountriesQueryHandler> localizer)
    : IRequestHandler<ExportCountriesQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        ExportCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var data = await context.Countries
            .AsNoTracking()
            .OrderBy(x => x.CountryNameEn)
            .ToListAsync(cancellationToken);

        var bytes = await excelService.DownloadDataAsync(data);
        return Result<byte[]>.Success(bytes, localizer["ExportedSuccessfully"]);
    }
}
