using BaridikExpress.Application.Features.LocationGeography.Dto.City;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.ImportCities;

public sealed class ImportCitiesCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ImportCitiesCommandHandler> localizer)
    : IRequestHandler<ImportCitiesCommand, Result<ExcelUploadResult<Domain.Entities.Location.City>>>
{
    public async Task<Result<ExcelUploadResult<Domain.Entities.Location.City>>> Handle(
        ImportCitiesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            return Result<ExcelUploadResult<Domain.Entities.Location.City>>.Failure(localizer["FileEmptyOrMissing"]);

        try
        {
            var countriesLookup = await context.Countries
                .AsNoTracking()
                .ToDictionaryAsync(
                    c => c.CountryNameEn.Trim().ToLowerInvariant(),
                    c => c.CountryId,
                    cancellationToken);

            var governmentsLookup = await context.Governments
                .AsNoTracking()
                .ToDictionaryAsync(
                    g => $"{g.CountryId}_{g.GovernmentNameEn.Trim().ToLowerInvariant()}",
                    g => g.GovernmentId,
                    cancellationToken);

            var result = await excelService.UploadAsync<CityExcelDto, Domain.Entities.Location.City>(
                request.File,

                mapper: dto =>
                {
                    if (string.IsNullOrWhiteSpace(dto.CityNameEn) ||
                        string.IsNullOrWhiteSpace(dto.CityNameAr))
                        throw new InvalidOperationException(localizer["NameRequired"]);

                    var countryKey = dto.CountryNameEn.Trim().ToLowerInvariant();
                    if (!countriesLookup.TryGetValue(countryKey, out var countryId))
                        throw new InvalidOperationException(
                            localizer["CountryNotFound", dto.CountryNameEn]);

                    var govKey = $"{countryId}_{dto.GovernmentNameEn.Trim().ToLowerInvariant()}";
                    if (!governmentsLookup.TryGetValue(govKey, out var governmentId))
                        throw new InvalidOperationException(
                            localizer["GovernmentNotFound", dto.GovernmentNameEn]);

                    return new Domain.Entities.Location.City
                    {
                        CityId = Guid.NewGuid(),
                        CityNameAr = dto.CityNameAr.Trim(),
                        CityNameEn = dto.CityNameEn.Trim(),
                        CountryId = countryId,
                        GovernmentId = governmentId
                    };
                },

                existsChecker: async entity =>
                    await context.Cities
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.GovernmentId == entity.GovernmentId &&
                            (x.CityNameEn == entity.CityNameEn ||
                             x.CityNameAr == entity.CityNameAr),
                            cancellationToken),

                inFileKeySelector: entity =>
                    $"{entity.GovernmentId}_{entity.CityNameEn}_{entity.CityNameAr}",

                cancellationToken: cancellationToken);

            return Result<ExcelUploadResult<Domain.Entities.Location.City>>
                .Success(result, localizer["ImportedSuccessfully"]);
        }
        catch (Exception ex)
        {
            return Result<ExcelUploadResult<Domain.Entities.Location.City>>
                .Failure(localizer["ImportFailed", ex.Message]);
        }
    }
}