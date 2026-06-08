using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands
{
    public class DeleteBlogsAuthorCommand : IRequest<Result<bool>>
    {
        public List<Guid> Ids { get; set; } = new();
    }
}