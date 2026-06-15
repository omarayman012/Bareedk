using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.ImportGovernments;

public sealed class ImportGovernmentsCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ImportGovernmentsCommandHandler> localizer)
    : IRequestHandler<ImportGovernmentsCommand, Result<ExcelUploadResult<Domain.Entities.Location.Government>>>
{
    public async Task<Result<ExcelUploadResult<Domain.Entities.Location.Government>>> Handle(
        ImportGovernmentsCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            return Result<ExcelUploadResult<Domain.Entities.Location.Government>>.Failure(localizer["FileEmptyOrMissing"]);

        try
        {
            var countriesLookup = await context.Countries
                .AsNoTracking()
                .ToDictionaryAsync(
                    c => c.CountryNameEn.Trim().ToLowerInvariant(),
                    c => c.CountryId,
                    cancellationToken);

            var result = await excelService.UploadAsync<GovernmentExcelDto, Domain.Entities.Location.Government>(
                request.File,

                mapper: dto =>
                {
                    if (string.IsNullOrWhiteSpace(dto.GovernmentNameEn) ||
                        string.IsNullOrWhiteSpace(dto.GovernmentNameAr))
                        throw new InvalidOperationException(localizer["NameRequired"]);

                    var countryKey = dto.CountryNameEn.Trim().ToLowerInvariant();
                    if (!countriesLookup.TryGetValue(countryKey, out var countryId))
                        throw new InvalidOperationException(
                            localizer["CountryNotFound", dto.CountryNameEn]);

                    return new Domain.Entities.Location.Government
                    {
                        GovernmentId = Guid.NewGuid(),
                        GovernmentNameAr = dto.GovernmentNameAr.Trim(),
                        GovernmentNameEn = dto.GovernmentNameEn.Trim(),
                        CountryId = countryId
                    };
                },

                existsChecker: async entity =>
                    await context.Governments
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.CountryId == entity.CountryId &&
                            (x.GovernmentNameEn == entity.GovernmentNameEn ||
                             x.GovernmentNameAr == entity.GovernmentNameAr),
                            cancellationToken),

                inFileKeySelector: entity =>
                    $"{entity.CountryId}_{entity.GovernmentNameEn}_{entity.GovernmentNameAr}",

                cancellationToken: cancellationToken);

            return Result<ExcelUploadResult<Domain.Entities.Location.Government>>
                .Success(result, localizer["ImportedSuccessfully"]);
        }
        catch (Exception ex)
        {
            return Result<ExcelUploadResult<Domain.Entities.Location.Government>>
                .Failure(localizer["ImportFailed", ex.Message]);
        }
    }
}