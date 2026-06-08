using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment
{
    public class UpdateCommentHandler(
     IApplicationDbContext context,
     IGetCurrentUserRepository currentUser,
     IStringLocalizer localizer)
     : IRequestHandler<UpdateCommentCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = currentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    return Result<bool>.Failure(localizer["UserNotAuthenticated"], 401);

                var comment = await context.BlogComments
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (comment == null)
                    return Result<bool>.Failure(localizer["CommentNotFound"], 404);

                if (comment.UserId != userId)
                    return Result<bool>.Failure(localizer["NoPermissionToUpdate"], 403);

                comment.Content = request.Content;

                await context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true, localizer["CommentUpdatedSuccessfully"]);
            }
            catch (Exception)
            {
                return Result<bool>.Failure(localizer["UnexpectedError"], 500);
            }
        }
    }
}
