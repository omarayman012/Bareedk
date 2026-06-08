
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler
{
    public class ToggleBlogsAuthorStatusCommandHandler
        : IRequestHandler<ToggleBlogsAuthorStatusCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public ToggleBlogsAuthorStatusCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            ToggleBlogsAuthorStatusCommand request,
            CancellationToken cancellationToken)
        {
            var author = await _context.BlogsAuthors
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (author == null)
            {
                return Result<bool>.Failure(
                    _localizer["BlogsAuthorNotFound"], 404);
            }

            author.IsActive = !author.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            var message = author.IsActive
                ? _localizer["BlogsAuthorActivated"]
                : _localizer["BlogsAuthorDeactivated"];

            return Result<bool>.Success(
                author.IsActive,
                message,
                200);
        }
    }
}