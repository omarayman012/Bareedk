using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Queries
{
    public class GetBlogsAuthorByIdQuery
        : IRequest<Result<GetBlogsAuthorByIdDto>>
    {
        public Guid Id { get; set; }
    }
}