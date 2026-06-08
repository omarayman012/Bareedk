using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;

public class ToggleBlogStatusCommand : IRequest<Result<bool>>
{
    public Guid BlogId { get; set; }
}
