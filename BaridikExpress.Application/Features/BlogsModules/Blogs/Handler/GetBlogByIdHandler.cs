
using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, Result<BlogDetailsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;
    private readonly IGetCurrentUserRepository _currentUser;

    public GetBlogByIdHandler(
        IApplicationDbContext context,
        IStringLocalizer localizer,
        IGetCurrentUserRepository currentUser)
    {
        _context = context;
        _localizer = localizer;
        _currentUser = currentUser;
    }

    public async Task<Result<BlogDetailsResponse>> Handle(
        GetBlogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var blog = await _context.Blogs
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(b => new
            {
                b.Id,
                b.TitleAr,
                b.TitleEn,
                b.DescriptionAr,
                b.DescriptionEn,
                b.Image,
                b.IsActive,
                b.CreatedAt,
                b.UpdatedAt,

                CreatedBy = b.CreatedBy != null
                    ? b.CreatedBy.FullName
                    : string.Empty,

                UpdatedBy = b.UpdatedBy != null
                    ? b.UpdatedBy.FullName
                    : null,

                Category = new
                {
                    b.Category.Id,
                    b.Category.NameAr,
                    b.Category.NameEn
                },

                Author = new
                {
                    b.Author.Id,
                    b.Author.NameAr,
                    b.Author.NameEn
                },

                Tags = b.BlogTags
                    .Where(t => t.Tag != null)
                    .Select(t => new
                    {
                        Ar = t.Tag.NameAr,
                        En = t.Tag.NameEn
                    })
                    .Distinct()
                    .OrderBy(t => t.En)
                    .ToList(),

                Seo = b.Seo == null
                    ? null
                    : new
                    {
                        b.Seo.MetaTitleAr,
                        b.Seo.MetaTitleEn,
                        b.Seo.SlugAr,
                        b.Seo.SlugEn,
                        b.Seo.MetaKeywordsAr,
                        b.Seo.MetaKeywordsEn,
                        b.Seo.MetaImage,
                        b.Seo.MetaDescriptionAr,
                        b.Seo.MetaDescriptionEn
                    },

                Likes = b.Reactions.Count(r => r.Type == ReactionType.Like),

                Dislikes = b.Reactions.Count(r => r.Type == ReactionType.Dislike),

                MyReaction = b.Reactions
                    .Where(r => r.UserId == userId)
                    .Select(r => (ReactionType?)r.Type)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (blog is null)
            return Result<BlogDetailsResponse>.Failure(
                _localizer["BlogNotFound"],
                404);

        var commentsBase = _context.BlogComments
            .AsNoTracking()
            .Where(x => x.BlogId == blog.Id && x.ParentId == null);

        var totalCommentsCount = await commentsBase.CountAsync(cancellationToken);

        var comments = await commentsBase
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.CommentsPageNumber - 1) * request.CommentsPageSize)
            .Take(request.CommentsPageSize)
            .Select(c => new CommentResponse
            {
                Id = c.Id,
                FullName = c.User.FullName,
                Photo = c.User.ProfileImageUrl ?? "NotProfileImage",
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                LikesCount = c.Reactions.Count(r => r.Type == ReactionType.Like),
                DislikesCount = c.Reactions.Count(r => r.Type == ReactionType.Dislike),
                RepliesCount = c.Replies.Count(),

                Replies = c.Replies
                    .Select(r => new CommentResponse
                    {
                        Id = r.Id,
                        FullName = r.User.FullName,
                        Photo = r.User.ProfileImageUrl ?? "NotProfileImage",
                        Content = r.Content,
                        CreatedAt = r.CreatedAt,
                        LikesCount = r.Reactions.Count(x => x.Type == ReactionType.Like),
                        DislikesCount = r.Reactions.Count(x => x.Type == ReactionType.Dislike)
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        var response = new BlogDetailsResponse
        {
            Id = blog.Id,

            Title = new NameDto
            {
                Ar = blog.TitleAr,
                En = blog.TitleEn
            },

            Description = new DescriptionDto
            {
                Ar = blog.DescriptionAr,
                En = blog.DescriptionEn
            },

            Image = blog.Image,
            IsActive = blog.IsActive,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            CreatedBy = blog.CreatedBy,
            UpdatedBy = blog.UpdatedBy,

            Category = new LookupDto
            {
                Id = blog.Category.Id,
                Name = new NameDto
                {
                    Ar = blog.Category.NameAr,
                    En = blog.Category.NameEn
                }
            },

            Author = new LookupDto
            {
                Id = blog.Author.Id,
                Name = new NameDto
                {
                    Ar = blog.Author.NameAr,
                    En = blog.Author.NameEn
                }
            },

            Tags = new TagsDto
            {
                Ar = blog.Tags
        .Where(x => !string.IsNullOrWhiteSpace(x.Ar))
        .Select(x => x.Ar!)
        .ToList(),

                En = blog.Tags
        .Where(x => !string.IsNullOrWhiteSpace(x.En))
        .Select(x => x.En!)
        .ToList()
            },

            Likes = blog.Likes,
            Dislikes = blog.Dislikes,
            MyReaction = blog.MyReaction,

            Seo = blog.Seo is null
                ? null
                : new BlogSeoResponse
                {
                    MetaTitle = new NameDto
                    {
                        Ar = blog.Seo.MetaTitleAr,
                        En = blog.Seo.MetaTitleEn
                    },

                    Slug = new SlugDto
                    {
                        Ar = string.IsNullOrWhiteSpace(blog.Seo.SlugAr)
                            ? new List<string>()
                            : blog.Seo.SlugAr
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .ToList(),

                        En = string.IsNullOrWhiteSpace(blog.Seo.SlugEn)
                            ? new List<string>()
                            : blog.Seo.SlugEn
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .ToList()
                    },

                    MetaKeywords = new MetaKeywordsDto
                    {
                        Ar = blog.Seo.MetaKeywordsAr?
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList() ?? new List<string>(),

                        En = blog.Seo.MetaKeywordsEn?
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList() ?? new List<string>()
                    },

                    MetaImage = blog.Seo.MetaImage,

                    MetaDescription = new DescriptionDto
                    {
                        Ar = blog.Seo.MetaDescriptionAr,
                        En = blog.Seo.MetaDescriptionEn
                    }
                },

            Comments = new PaginatedList<CommentResponse>(
                comments,
                request.CommentsPageNumber,
                totalCommentsCount,
                request.CommentsPageSize)
        };

        return Result<BlogDetailsResponse>.Success(
            response,
            _localizer["BlogRetrievedSuccessfully"]);
    }
}