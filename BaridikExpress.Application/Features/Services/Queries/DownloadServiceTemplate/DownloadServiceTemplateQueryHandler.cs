using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Application.Interfaces.File;
using MediatR;

namespace BaridikExpress.Application.Features.Services.Queries.DownloadServiceTemplate;

public sealed class DownloadServiceTemplateQueryHandler(
    IExcelService excelService)
    : IRequestHandler<DownloadServiceTemplateQuery, byte[]>
{
    public async Task<byte[]> Handle(
        DownloadServiceTemplateQuery request,
        CancellationToken cancellationToken)
    {
        return await excelService
            .DownloadTemplateAsync<ServiceExcelDto>();
    }
}