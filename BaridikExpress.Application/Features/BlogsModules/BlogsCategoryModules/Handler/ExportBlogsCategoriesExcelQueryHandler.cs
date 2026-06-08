
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class ExportBlogsCategoriesExcelQueryHandler
        : IRequestHandler<ExportBlogsCategoriesExcelQuery, Result<byte[]>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer _localizer;

        public ExportBlogsCategoriesExcelQueryHandler(
            IApplicationDbContext applicationDbContext,
            IExcelService excelService,
            IStringLocalizer localizer)
        {
            _applicationDbContext = applicationDbContext;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<byte[]>> Handle(
            ExportBlogsCategoriesExcelQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var blogsCategories = await _applicationDbContext.BlogsCategorys
                    .AsNoTracking()
                    .Select(category => new ExportBlogsCategoryExcelDto
                    {
                        NameAr = category.NameAr ?? string.Empty,
                        NameEn = category.NameEn ?? string.Empty,
                        Priorty = category.Priorty,
                        DescriptionAr = category.DescriptionAr,
                        DescriptionEn = category.DescriptionEn,
                        IsActive = category.IsActive,
                        BlogsCount = category.Blogs.Count(),
                        CreatedAt = category.CreatedAt
                    })
                    .ToListAsync(cancellationToken);

                var fileBytes = await _excelService.DownloadDataAsync(blogsCategories);

                return Result<byte[]>.Success(
                    fileBytes,
                    _localizer["BlogsCategoriesExportedSuccessfully"],
                    200);
            }
            catch
            {
                return Result<byte[]>.Error(
                    _localizer["FailedToExportBlogsCategories"],
                    500);
            }
        }
    }
}