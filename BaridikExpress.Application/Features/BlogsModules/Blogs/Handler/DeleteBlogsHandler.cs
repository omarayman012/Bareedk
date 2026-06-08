
using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class DeleteBlogsHandler(IApplicationDbContext context, IStringLocalizer localizer) : IRequestHandler<DeleteBlogsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBlogsCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids == null || !request.Ids.Any())
            return Result<bool>.Failure(localizer["NoIdsProvided"], 400);

        var blogs = await context.Blogs
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!blogs.Any())
            return Result<bool>.Failure(localizer["BlogsNotFound"], 404);

        context.Blogs.RemoveRange(blogs);

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["BlogsDeletedSuccessfully"]);
    }
}
