
using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules
{
    public class GetBlogReactionsHandler : IRequestHandler<GetBlogReactionsQuery, Result<BlogReactionsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public GetBlogReactionsHandler(IApplicationDbContext context, IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<BlogReactionsResponse>> Handle(GetBlogReactionsQuery request, CancellationToken cancellationToken)
        {
            var exists = await _context.Blogs
                .AnyAsync(x => x.Id == request.BlogId, cancellationToken);

            if (!exists)
                return Result<BlogReactionsResponse>.Failure(_localizer["BlogNotFound"], 404);

            var reactions = await _context.BlogReactions
                .Where(x => x.BlogId == request.BlogId)
                .Select(x => new
                {
                    x.Type,
                    x.UserId,
                    x.User.FullName,
                    x.User.ProfileImageUrl
                })
                .ToListAsync(cancellationToken);

            var response = new BlogReactionsResponse
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

            return Result<BlogReactionsResponse>.Success(response, _localizer["ReactionsRetrievedSuccessfully"]);
        }
    }
}
