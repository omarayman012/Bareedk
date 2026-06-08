using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class ToggleBlogReactionHandler(
    IApplicationDbContext context,
    IGetCurrentUserRepository currentUser,
    IStringLocalizer localizer)
    : IRequestHandler<ToggleBlogReactionCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ToggleBlogReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUser.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
                return Result<bool>.Failure(localizer["UserNotAuthenticated"], 401);

            var blogExists = await context.Blogs
                .AnyAsync(x => x.Id == request.BlogId, cancellationToken);

            if (!blogExists)
                return Result<bool>.Failure(localizer["BlogNotFound"], 404);

            var existing = await context.BlogReactions
                .FirstOrDefaultAsync(x =>
                    x.BlogId == request.BlogId &&
                    x.UserId == userId,
                    cancellationToken);

            if (existing == null)
            {
                context.BlogReactions.Add(new BlogReaction
                {
                    Id = Guid.NewGuid(),
                    BlogId = request.BlogId,
                    UserId = userId,   
                    Type = request.Type
                });
            }
            else if (existing.Type == request.Type)
            {
                context.BlogReactions.Remove(existing);
            }
            else
            {
                existing.Type = request.Type;
            }
            await context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, localizer["ReactionUpdatedSuccessfully"]);
        }
        catch (DbUpdateException)
        {
            return Result<bool>.Failure(localizer["DatabaseError"], 500);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(localizer["UnexpectedError"], 500);
        }
    }
}