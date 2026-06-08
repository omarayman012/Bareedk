using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries
{
    public class ExportBlogsCategoriesExcelQuery : IRequest<Result<byte[]>>
    {
    }
}