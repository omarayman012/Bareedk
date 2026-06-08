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
    public sealed class GetCommentsExportQueryHandler(
       IApplicationDbContext context,
       IStringLocalizer localizer)
       : IRequestHandler<GetCommentsExportQuery, Result<List<CommentExportDto>>>
    {
        public async Task<Result<List<CommentExportDto>>> Handle(
            GetCommentsExportQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var blogExists = await context.Blogs
                    .AnyAsync(x => x.Id == request.BlogId, cancellationToken);

                if (!blogExists)
                    return Result<List<CommentExportDto>>
                        .Failure(localizer["BlogNotFound"], 404);

                var list = await context.BlogComments
                    .AsNoTracking()
                    .Include(c => c.User)
                    .Include(c => c.Replies)
                    .Include(c => c.Reactions)
                    .Where(x => x.BlogId == request.BlogId && x.ParentId == null)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(c => new CommentExportDto
                    {
                        FullName = c.User.FullName,
                        Content = c.Content,
                        LikesCount = c.Reactions.Count(r => r.Type == ReactionType.Like),
                        DislikesCount = c.Reactions.Count(r => r.Type == ReactionType.Dislike),
                        RepliesCount = c.Replies.Count,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<CommentExportDto>>
                    .Success(list, localizer["DataFetchedSuccessfully"], 200);
            }
            catch (Exception ex)
            {
                return Result<List<CommentExportDto>>
                    .Error(localizer["UnexpectedError"] + $": {ex.Message}", 500);
            }
        }
    }
}
