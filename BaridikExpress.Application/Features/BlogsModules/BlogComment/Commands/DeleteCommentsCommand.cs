using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;

public class DeleteCommentsCommand : IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = new();
}
