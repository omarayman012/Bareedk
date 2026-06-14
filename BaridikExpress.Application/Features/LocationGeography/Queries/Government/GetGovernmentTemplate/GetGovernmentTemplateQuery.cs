using BaridikExpress.Application.Features.LocationGeography.Dto.Government;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetGovernmentTemplate;

public sealed record GetGovernmentTemplateQuery : IRequest<Result<byte[]>>;

public sealed class GetGovernmentTemplateQueryHandler(
    IExcelService excelService,
    IStringLocalizer<GetGovernmentTemplateQueryHandler> localizer)
    : IRequestHandler<GetGovernmentTemplateQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(
        GetGovernmentTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var bytes = await excelService.GenerateTemplateAsync<GovernmentExcelDto>();
        return Result<byte[]>.Success(bytes, localizer["TemplateGeneratedSuccessfully"]);
    }
}
