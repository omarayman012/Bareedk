using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;

public class UpdateCommentCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string Content { get; set; } 

}
