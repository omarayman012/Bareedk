using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries
{
    public class DownloadBlogsCategoryTemplateQuery : IRequest<Result<byte[]>>
    {
    }
}