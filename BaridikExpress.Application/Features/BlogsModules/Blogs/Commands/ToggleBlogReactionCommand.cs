using BaridikExpress.Domain.Enum;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;

public class ToggleBlogReactionCommand : IRequest<Result<bool>>
{
    public Guid BlogId { get; set; }
    public ReactionType Type { get; set; }
}
