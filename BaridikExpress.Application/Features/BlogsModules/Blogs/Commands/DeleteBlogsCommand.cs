using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;

public class DeleteBlogsCommand : IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = new();
}
