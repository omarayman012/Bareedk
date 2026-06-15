using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.ImportCountries;

public sealed class ImportCountriesCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ImportCountriesCommandHandler> localizer)
    : IRequestHandler<ImportCountriesCommand, Result<ExcelUploadResult<Domain.Entities.Location.Country>>>
{
    public async Task<Result<ExcelUploadResult<Domain.Entities.Location.Country>>> Handle(
        ImportCountriesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            return Result<ExcelUploadResult<Domain.Entities.Location.Country>>.Failure(localizer["FileEmptyOrMissing"]);

        try
        {
            var result = await excelService.UploadAsync<CountryExcelDto, Domain.Entities.Location.Country>(
                request.File,

                mapper: dto =>
                {
                    if (string.IsNullOrWhiteSpace(dto.CountryNameEn) ||
                        string.IsNullOrWhiteSpace(dto.CountryNameAr))
                        throw new InvalidOperationException(localizer["NameRequired"]);

                    return new Domain.Entities.Location.Country
                    {
                        CountryId = Guid.NewGuid(),
                        CountryNameAr = dto.CountryNameAr.Trim(),
                        CountryNameEn = dto.CountryNameEn.Trim(),
                        PhoneCode = dto.PhoneCode.Trim(),
                        PostalCode = string.IsNullOrWhiteSpace(dto.PostalCode)
                            ? null
                            : dto.PostalCode.Trim()
                    };
                },

                existsChecker: async entity =>
                    await context.Countries
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.CountryNameEn == entity.CountryNameEn ||
                            x.CountryNameAr == entity.CountryNameAr,
                            cancellationToken),

                inFileKeySelector: entity =>
                    $"{entity.CountryNameEn}_{entity.CountryNameAr}",

                cancellationToken: cancellationToken);

            return Result<ExcelUploadResult<Domain.Entities.Location.Country>>
                .Success(result, localizer["ImportedSuccessfully"]);
        }
        catch (Exception ex)
        {
            return Result<ExcelUploadResult<Domain.Entities.Location.Country>>
                .Failure(localizer["ImportFailed", ex.Message]);
        }
    }
}