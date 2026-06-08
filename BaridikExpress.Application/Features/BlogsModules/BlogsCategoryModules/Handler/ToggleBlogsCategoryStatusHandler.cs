
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class ToggleBlogsCategoryActiveCommandHandler
        : IRequestHandler<ToggleBlogsCategoryStatusCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public ToggleBlogsCategoryActiveCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            ToggleBlogsCategoryStatusCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _context.BlogsCategorys
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
                return Result<bool>.Failure(_localizer["BlogsCategoryNotFound"], 404);

            category.IsActive = !category.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            var message = category.IsActive
                ? _localizer["BlogsCategoryActivated"]
                : _localizer["BlogsCategoryDeactivated"];

            return Result<bool>.Success(
                category.IsActive,
                message,
                200);
        }
    }
}
