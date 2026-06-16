using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Services.Commands.ImportServices;

public sealed class ImportServicesCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context,
    IStringLocalizer<ImportServicesCommandHandler> localizer)
    : IRequestHandler<ImportServicesCommand,
        Result<ExcelUploadResult<Service>>>
{
    public async Task<Result<ExcelUploadResult<Service>>> Handle(
        ImportServicesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
            return Result<ExcelUploadResult<Service>>
                .Failure(localizer["FileEmptyOrMissing"]);

        try
        {
            var result = await excelService.UploadAsync<ServiceExcelDto, Service>(
                request.File,

                mapper: dto =>
                {
                    if (string.IsNullOrWhiteSpace(dto.NameEn) ||
                        string.IsNullOrWhiteSpace(dto.NameAr))
                        throw new InvalidOperationException(
                            localizer["NameEnAndNameArRequired"]);

                    return Service.Create(
                        dto.NameEn,
                        dto.NameAr,
                        dto.Price,
                        dto.Currency,
                        dto.ImageUrl);
                },

                existsChecker: async service =>
                    await context.Services
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.NameEn == service.NameEn ||
                            x.NameAr == service.NameAr,
                            cancellationToken),

                inFileKeySelector: service =>
                    $"{service.NameEn}_{service.NameAr}",

                cancellationToken: cancellationToken);

            return Result<ExcelUploadResult<Service>>
                .Success(result, localizer["ServicesImportedSuccessfully"]);
        }
        catch (Exception ex)
        {
            return Result<ExcelUploadResult<Service>>
                .Failure(localizer["ImportFailed", ex.Message]);
        }
    }
}