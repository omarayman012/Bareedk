
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment;

public class GetAllCommentsQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer)
    : IRequestHandler<GetCommentsQuery, Result<PaginatedList<CommentResponse>>>
{
    public async Task<Result<PaginatedList<CommentResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var blogExists = await context.Blogs
                .AnyAsync(x => x.Id == request.BlogId, cancellationToken);

            if (!blogExists)
                return Result<PaginatedList<CommentResponse>>
                    .Failure(localizer["BlogNotFound"], 404);

            var baseQuery = context.BlogComments
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .Where(x => x.BlogId == request.BlogId && x.ParentId == null);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim().ToLower();
                baseQuery = baseQuery.Where(x =>
                    x.Content.ToLower().Contains(search) ||
                    x.User.FullName.ToLower().Contains(search) ||
                    x.Replies.Any(r => r.Content.ToLower().Contains(search)) ||
                    x.Replies.Any(r => r.User.FullName.ToLower().Contains(search))
                );
            }

            var orderedQuery = baseQuery.OrderByDescending(x => x.CreatedAt);

            var count = await orderedQuery.CountAsync(cancellationToken);

            if (count == 0)
                return Result<PaginatedList<CommentResponse>>
                    .Success(
                        new PaginatedList<CommentResponse>(new List<CommentResponse>(), request.PageNumber, 0, request.PageSize),
                        localizer["NoCommentsFound"]);

            var comments = await orderedQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToListAsync(cancellationToken);

            var result = new PaginatedList<CommentResponse>(
                comments,
                request.PageNumber,
                count,
                request.PageSize
            );

            return Result<PaginatedList<CommentResponse>>
                .Success(result, localizer["CommentsRetrievedSuccessfully"]);
        }
        catch (DbUpdateException)
        {
            return Result<PaginatedList<CommentResponse>>
                .Failure(localizer["DatabaseError"], 500);
        }
        catch (Exception)
        {
            return Result<PaginatedList<CommentResponse>>
                .Failure(localizer["UnexpectedError"], 500);
        }
    }
}