using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands
{
    public class ToggleBlogsAuthorStatusCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}