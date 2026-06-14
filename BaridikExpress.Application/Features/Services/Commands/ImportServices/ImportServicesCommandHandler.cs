using BaridikExpress.Application.Common.Abstractions;

using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Commands.ImportServices;

public sealed class ImportServicesCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context)
    : IRequestHandler<ImportServicesCommand,
        Result<ExcelUploadResult<Service>>>
{
    public async Task<Result<ExcelUploadResult<Service>>> Handle(
        ImportServicesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await excelService.UploadAsync<
            ServiceExcelDto,
            Service>(
            request.File,

            mapper: dto => Service.Create(
                dto.NameEn,
                dto.NameAr,
                dto.Price,
                dto.Currency,
                dto.ImageUrl),

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
            .Success(result, "Services imported successfully");
    }
}