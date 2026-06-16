using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.DownloadTemplate;

public sealed class DownloadServiceBusinessPlanTemplateQueryHandler(
IExcelService excelService)
: IRequestHandler<DownloadServiceBusinessPlanTemplateQuery, byte[]>
{
    public async Task<byte[]> Handle(
    DownloadServiceBusinessPlanTemplateQuery request,
    CancellationToken cancellationToken)
    {
        return await excelService
        .GenerateTemplateAsync<ServiceBusinessPlanExcelDto>();
    }
}
