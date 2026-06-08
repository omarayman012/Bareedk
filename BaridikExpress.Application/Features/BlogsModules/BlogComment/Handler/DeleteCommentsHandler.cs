
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment;

public class DeleteCommentsHandler(
 IApplicationDbContext context,
 IGetCurrentUserRepository currentUser,
 IStringLocalizer localizer)
 : IRequestHandler<DeleteCommentsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCommentsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUser.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
                return Result<bool>.Failure(localizer["UserNotAuthenticated"], 401);

            var comments = await context.BlogComments
                .Include(x => x.Blog)
                .Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!comments.Any())
                return Result<bool>.Failure(localizer["CommentsNotFound"], 404);

            var allowedComments = comments.Where(c =>
                c.UserId == userId 
                || c.Blog.CreatedById == userId 
            ).ToList();

            if (!allowedComments.Any())
                return Result<bool>.Failure(localizer["NoPermissionToDelete"], 403);

            context.BlogComments.RemoveRange(allowedComments);

            await context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, localizer["CommentsDeletedSuccessfully"]);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(localizer["UnexpectedError"], 500);
        }
    }
}
