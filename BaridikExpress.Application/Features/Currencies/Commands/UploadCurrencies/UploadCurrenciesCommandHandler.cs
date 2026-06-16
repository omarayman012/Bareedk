using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Features.Currencies.DTO;
using BaridikExpress.Domain.Entities.CurrencyModule;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.UploadCurrencies;

public class UploadCurrenciesCommandHandler
    : IRequestHandler<UploadCurrenciesCommand, Result<ExcelUploadResult<Currency>>>
{
    private readonly IExcelService _excelService;
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public UploadCurrenciesCommandHandler(
        IExcelService excelService,
        IApplicationDbContext context,
        IStringLocalizer localizer)
    {
        _excelService = excelService;
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<ExcelUploadResult<Currency>>> Handle(
        UploadCurrenciesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _excelService.UploadAsync<CurrencyExcelImportDto, Currency>(
            request.File,
            mapper: dto => new Currency(
                dto.NameEn,
                dto.NameAr,
                dto.CurrencyCode,
                dto.CurrencySymbol,
                dto.IsActive
            ),
            existsChecker: async entity =>
                await _context.Currencies
                    .AnyAsync(x => x.CurrencyCode.ToLower() == entity.CurrencyCode.ToLower()),
            inFileKeySelector: entity => entity.CurrencyCode,
            cancellationToken: cancellationToken
        );

        return Result<ExcelUploadResult<Currency>>
            .Success(result, _localizer["CurrenciesImportedSuccessfully"]);
    }
}