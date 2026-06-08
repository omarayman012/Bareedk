
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class DownloadBlogsCategoryTemplateQueryHandler
        : IRequestHandler<DownloadBlogsCategoryTemplateQuery, Result<byte[]>>
    {
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer _localizer;

        public DownloadBlogsCategoryTemplateQueryHandler(
            IExcelService excelService,
            IStringLocalizer localizer)
        {
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<byte[]>> Handle(
            DownloadBlogsCategoryTemplateQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var fileBytes = await _excelService.DownloadTemplateAsync<ImportBlogsCategoryExcelDto>();

                return Result<byte[]>.Success(
                    fileBytes,
                    _localizer["BlogsCategoryTemplateDownloadedSuccessfully"],
                    200);
            }
            catch
            {
                return Result<byte[]>.Error(
                    _localizer["FailedToDownloadBlogsCategoryTemplate"],
                    500);
            }
        }
    }
}