using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.LocationGeography.Dto.Village;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.ImportVillages;

public sealed class ImportVillagesCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ImportVillagesCommandHandler> localizer)
    : IRequestHandler<ImportVillagesCommand, Result<ExcelUploadResult<Village>>>
{
    public async Task<Result<ExcelUploadResult<Village>>> Handle(
        ImportVillagesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            return Result<ExcelUploadResult<Village>>.Failure(localizer["FileEmptyOrMissing"]);

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

            var citiesLookup = await context.Cities
                .AsNoTracking()
                .ToDictionaryAsync(
                    c => $"{c.GovernmentId}_{c.CityNameEn.Trim().ToLowerInvariant()}",
                    c => c.CityId,
                    cancellationToken);

            var result = await excelService.UploadAsync<VillageExcelDto, Village>(
                request.File,

                mapper: dto =>
                {
                    if (string.IsNullOrWhiteSpace(dto.VillageNameEn) ||
                        string.IsNullOrWhiteSpace(dto.VillageNameAr))
                        throw new InvalidOperationException(localizer["NameRequired"]);

                    var countryKey = dto.CountryNameEn.Trim().ToLowerInvariant();
                    if (!countriesLookup.TryGetValue(countryKey, out var countryId))
                        throw new InvalidOperationException(
                            localizer["CountryNotFound", dto.CountryNameEn]);

                    var govKey = $"{countryId}_{dto.GovernmentNameEn.Trim().ToLowerInvariant()}";
                    if (!governmentsLookup.TryGetValue(govKey, out var governmentId))
                        throw new InvalidOperationException(
                            localizer["GovernmentNotFound", dto.GovernmentNameEn]);

                    var cityKey = $"{governmentId}_{dto.CityNameEn.Trim().ToLowerInvariant()}";
                    if (!citiesLookup.TryGetValue(cityKey, out var cityId))
                        throw new InvalidOperationException(
                            localizer["CityNotFound", dto.CityNameEn]);

                    return new Village
                    {
                        VillageId = Guid.NewGuid(),
                        VillageNameAr = dto.VillageNameAr.Trim(),
                        VillageNameEn = dto.VillageNameEn.Trim(),
                        CountryId = countryId,
                        GovernmentId = governmentId,
                        CityId = cityId
                    };
                },

                existsChecker: async entity =>
                    await context.Villages
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.CityId == entity.CityId &&
                            (x.VillageNameEn == entity.VillageNameEn ||
                             x.VillageNameAr == entity.VillageNameAr),
                            cancellationToken),

                inFileKeySelector: entity =>
                    $"{entity.CityId}_{entity.VillageNameEn}_{entity.VillageNameAr}",

                cancellationToken: cancellationToken);

            return Result<ExcelUploadResult<Village>>
                .Success(result, localizer["ImportedSuccessfully"]);
        }
        catch (Exception ex)
        {
            return Result<ExcelUploadResult<Village>>
                .Failure(localizer["ImportFailed", ex.Message]);
        }
    }
}