
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment;

public class GetCommentByIdHandler(
 IApplicationDbContext context,
 IStringLocalizer localizer)
 : IRequestHandler<GetCommentByIdQuery, Result<CommentResponse>>
{
    public async Task<Result<CommentResponse>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var comment = await context.BlogComments
                .AsNoTracking()
                .Where(x => x.Id == request.CommentId)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    FullName = c.User.FullName,
                    Photo = c.User.ProfileImageUrl ?? "NotProfileImage",
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,

                    LikesCount = c.Reactions.Count(r => r.Type == ReactionType.Like),
                    DislikesCount = c.Reactions.Count(r => r.Type == ReactionType.Dislike),

                    RepliesCount = c.Replies.Count,

                    Replies = c.Replies.Select(r => new CommentResponse
                    {
                        Id = r.Id,
                        FullName = r.User.FullName,
                        Photo = r.User.ProfileImageUrl ?? "NotProfileImage",
                        Content = r.Content,
                        CreatedAt = r.CreatedAt,
                        LikesCount = r.Reactions.Count(x => x.Type == ReactionType.Like),
                        DislikesCount = r.Reactions.Count(x => x.Type == ReactionType.Dislike)
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (comment == null)
                return Result<CommentResponse>
                    .Failure(localizer["CommentNotFound"], 404);

            return Result<CommentResponse>
                .Success(comment, localizer["CommentRetrievedSuccessfully"]);
        }
        catch (Exception)
        {
            return Result<CommentResponse>
                .Failure(localizer["UnexpectedError"], 500);
        }
    }
}
