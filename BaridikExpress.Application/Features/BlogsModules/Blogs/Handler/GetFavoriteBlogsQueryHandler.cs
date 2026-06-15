using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Handler
{
    public class GetFavoriteBlogsQueryHandler
    : IRequestHandler<GetFavoriteBlogsQuery, Result<PaginatedList<FavoriteBlogDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGetCurrentUserRepository _currentUserService;

        public GetFavoriteBlogsQueryHandler(
            IApplicationDbContext context,
            IGetCurrentUserRepository currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedList<FavoriteBlogDto>>> Handle(
            GetFavoriteBlogsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var query = _context.BlogReactions
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    x.Type == ReactionType.Like)
                .Select(x => x.Blog)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim().ToLower();

                query = query.Where(x =>
                    x.TitleAr.ToLower().Contains(search) ||
                    x.TitleEn.ToLower().Contains(search));
            }

            var favoriteBlogsQuery = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new FavoriteBlogDto
                {
                    Id = x.Id,

                    Title = new NameDto
                    {
                        Ar = x.TitleAr,
                        En = x.TitleEn
                    },

                    Description = new DescriptionDto
                    {
                        Ar = x.DescriptionAr,
                        En = x.DescriptionEn
                    },

                    Image = x.Image,

                    Category = new LookupDto
                    {
                        Id = x.Category.Id,
                        Name = new NameDto
                        {
                            Ar = x.Category.NameAr,
                            En = x.Category.NameEn
                        }
                    },

                    Author = new LookupDto
                    {
                        Id = x.Author.Id,
                        Name = new NameDto
                        {
                            Ar = x.Author.NameAr,
                            En = x.Author.NameEn
                        }
                    },

                    CreatedDate = x.CreatedDate,
                    CreatedTime = x.CreatedTime,

                    CommentsCount = x.Comments.Count(),

                    ReactionsCount = x.Reactions.Count()
                });

            var paginatedList = await PaginatedList<FavoriteBlogDto>.CreateAsync(
                favoriteBlogsQuery,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedList<FavoriteBlogDto>>.Success(paginatedList);
        }
    }
}
