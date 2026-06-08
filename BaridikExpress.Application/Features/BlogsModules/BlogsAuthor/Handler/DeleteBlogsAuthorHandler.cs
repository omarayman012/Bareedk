
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler
{
    public class DeleteBlogsAuthorHandler
        : IRequestHandler<DeleteBlogsAuthorCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public DeleteBlogsAuthorHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            DeleteBlogsAuthorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.Ids == null || !request.Ids.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["IdsRequired"], 400);
                }

                var authors = await _context.BlogsAuthors
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                if (!authors.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["BlogsAuthorNotFound"], 404);
                }

                _context.BlogsAuthors.RemoveRange(authors);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    _localizer["BlogsAuthorDeletedSuccessfully"],
                    200);
            }
            catch (Exception)
            {
                return Result<bool>.Error(
                    _localizer["UnexpectedError"],
                    500);
            }
        }
    }
}