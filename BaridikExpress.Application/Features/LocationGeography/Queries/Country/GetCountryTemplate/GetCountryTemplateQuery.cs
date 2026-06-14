using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetCountryTemplate;

public sealed record GetCountryTemplateQuery : IRequest<Result<byte[]>>;

public sealed class GetCountryTemplateQueryHandler(
    IExcelService excelService,
    IStringLocalizer<GetCountryTemplateQueryHandler> localizer)
    : IRequestHandler<GetCountryTemplateQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        GetCountryTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var bytes = await excelService.GenerateTemplateAsync<CountryExcelDto>();
        return Result<byte[]>.Success(bytes, localizer["TemplateGeneratedSuccessfully"]);
    }
}