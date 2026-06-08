using BaridikExpress.Domain.Enum;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;

public class ToggleCommentReactionCommand : IRequest<Result<int>>
{
    public Guid CommentId { get; set; }
    public ReactionType Type { get; set; }
}
