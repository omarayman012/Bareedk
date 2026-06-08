using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
public class GetBlogsQuery : IRequest<Result<PaginatedList<BlogListResponse>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public Guid? BlogCategoryId { get; set; }       
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}