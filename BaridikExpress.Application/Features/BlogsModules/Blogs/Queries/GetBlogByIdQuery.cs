using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
public class GetBlogByIdQuery : IRequest<Result<BlogDetailsResponse>>
{
    public Guid Id { get; set; }

    public int CommentsPageNumber { get; set; } = 1;
    public int CommentsPageSize { get; set; } = 10;
}
