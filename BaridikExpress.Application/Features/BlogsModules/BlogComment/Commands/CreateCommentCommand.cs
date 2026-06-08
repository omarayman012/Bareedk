using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;

public class CreateCommentCommand : IRequest<Result<Guid>>
{
    public Guid BlogId { get; set; }
    public string Content { get; set; }
    public Guid? ParentId { get; set; } 
}
