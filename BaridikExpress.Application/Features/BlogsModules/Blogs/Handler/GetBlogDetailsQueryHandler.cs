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

    public class GetBlogDetailsQueryHandler
        : IRequestHandler<GetBlogDetailsQuery, Result<BlogDetailsDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBlogDetailsQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<BlogDetailsDto>> Handle(
            GetBlogDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs
                .AsNoTracking()
                .Where(x => x.Id == request.BlogId)
                .Select(x => new BlogDetailsDto
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

                    ReactionsCount = x.Reactions.Count(),

                    Tags = x.BlogTags
                        .Select(t => new LookupDto
                        {
                            Id = t.Tag.Id,
                            Name = new NameDto
                            {
                                Ar = t.Tag.NameAr,
                                En = t.Tag.NameEn
                            }
                        })
                        .ToList(),

                    Comments = x.Comments
                        .Where(c => c.ParentId == null)
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => new CommentDto
                        {
                            Id = c.Id,

                            UserName = c.User.FullName,

                            Content = c.Content,

                            CreatedAt = c.CreatedAt,

                            LikesCount = c.Reactions
                                .Count(r => r.Type == ReactionType.Like),

                            DislikesCount = c.Reactions
                                .Count(r => r.Type == ReactionType.Dislike),

                            RepliesCount = c.Replies.Count(),

                            Replies = c.Replies
                                .OrderBy(r => r.CreatedAt)
                                .Select(r => new CommentReplyDto
                                {
                                    Id = r.Id,

                                    UserName = r.User.FullName,

                                    Content = r.Content,

                                    CreatedAt = r.CreatedAt,

                                    LikesCount = r.Reactions
                                        .Count(x => x.Type == ReactionType.Like),

                                    DislikesCount = r.Reactions
                                        .Count(x => x.Type == ReactionType.Dislike)
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (blog is null)
            {
                return Result<BlogDetailsDto>.Failure("Blog not found");
            }

            return Result<BlogDetailsDto>.Success(blog);
        }
    }
}
