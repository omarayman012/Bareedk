using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands
{
    public class ImportBlogsCategoriesExcelCommand
        : IRequest<Result<ImportBlogsCategoryExcelResultDto>>
    {
        public IFormFile File { get; set; } = default!;
    }
}