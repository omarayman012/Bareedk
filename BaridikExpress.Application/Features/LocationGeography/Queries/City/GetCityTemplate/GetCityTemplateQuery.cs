using BaridikExpress.Application.Features.LocationGeography.Dto.City;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetCityTemplate;

public sealed record GetCityTemplateQuery : IRequest<Result<byte[]>>;

public sealed class GetCityTemplateQueryHandler(
    IExcelService excelService,
    IStringLocalizer<GetCityTemplateQueryHandler> localizer)
    : IRequestHandler<GetCityTemplateQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        GetCityTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var bytes = await excelService.GenerateTemplateAsync<CityExcelDto>();
        return Result<byte[]>.Success(bytes, localizer["TemplateGeneratedSuccessfully"]);
    }
}