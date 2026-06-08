using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment
{
    public class GetCommentReactionsHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer)
    : IRequestHandler<GetCommentReactionsQuery, Result<CommentReactionsResponse>>
    {
        public async Task<Result<CommentReactionsResponse>> Handle(GetCommentReactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await context.BlogComments
                    .AnyAsync(x => x.Id == request.CommentId, cancellationToken);

                if (!exists)
                    return Result<CommentReactionsResponse>
                        .Failure(localizer["CommentNotFound"], 404);

                var reactions = await context.CommentReactions
                    .Where(x => x.CommentId == request.CommentId)
                    .Select(x => new
                    {
                        x.Type,
                        x.UserId,
                        x.User.FullName,
                        x.User.ProfileImageUrl
                    })
                    .ToListAsync(cancellationToken);

                var response = new CommentReactionsResponse
                {
                    Likes = reactions
                        .Where(x => x.Type == ReactionType.Like)
                        .Select(x => new UserReactionDto
                        {
                            UserId = x.UserId,
                            FullName = x.FullName,
                            Photo = x.ProfileImageUrl
                        }).ToList(),

                    Dislikes = reactions
                        .Where(x => x.Type == ReactionType.Dislike)
                        .Select(x => new UserReactionDto
                        {
                            UserId = x.UserId,
                            FullName = x.FullName,
                            Photo = x.ProfileImageUrl
                        }).ToList()
                };

                return Result<CommentReactionsResponse>
                    .Success(response, localizer["ReactionsRetrievedSuccessfully"]);
            }
            catch (Exception)
            {
                return Result<CommentReactionsResponse>
                    .Failure(localizer["UnexpectedError"], 500);
            }
        }
    }
}
