
using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class ToggleBlogStatusHandler : IRequestHandler<ToggleBlogStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public ToggleBlogStatusHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(ToggleBlogStatusCommand request, CancellationToken cancellationToken)
    {
        var blog = await _context.Blogs
            .FirstOrDefaultAsync(x => x.Id == request.BlogId, cancellationToken);

        if (blog == null)
            return Result<bool>.Failure(_localizer["Blognotfound"], 404);

        blog.IsActive = !blog.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        var message = blog.IsActive
            ? _localizer["Blogactivatedsuccessfully"]
            : _localizer["Blogdeactivatedsuccessfully"];

        return Result<bool>.Success(blog.IsActive, message);
    }
}
