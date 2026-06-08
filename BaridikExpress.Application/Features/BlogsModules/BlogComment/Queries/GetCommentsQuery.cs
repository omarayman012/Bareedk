using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;


namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;
public class GetCommentsQuery : IRequest<Result<PaginatedList<CommentResponse>>>
{
    public Guid BlogId { get; set; }
    public string? Name { get; set; } 
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}