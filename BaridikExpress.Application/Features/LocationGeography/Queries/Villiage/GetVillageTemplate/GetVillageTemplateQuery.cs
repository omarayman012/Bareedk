using BaridikExpress.Application.Features.LocationGeography.Dto.Village;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetVillageTemplate;

public sealed record GetVillageTemplateQuery : IRequest<Result<byte[]>>;

public sealed class GetVillageTemplateQueryHandler(
    IExcelService excelService,
    IStringLocalizer<GetVillageTemplateQueryHandler> localizer)
    : IRequestHandler<GetVillageTemplateQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        GetVillageTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var bytes = await excelService.GenerateTemplateAsync<VillageExcelDto>();
        return Result<byte[]>.Success(bytes, localizer["TemplateGeneratedSuccessfully"]);
    }
}