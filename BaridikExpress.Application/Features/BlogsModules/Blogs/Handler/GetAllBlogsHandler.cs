
using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Entities.BlogsModules;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class GetAllBlogsHandler : IRequestHandler<GetBlogsQuery, Result<PaginatedList<BlogListResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public GetAllBlogsHandler(
        IApplicationDbContext context,
        IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<PaginatedList<BlogListResponse>>> Handle(
        GetBlogsQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = BuildQuery(request);

        var count = await baseQuery.CountAsync(cancellationToken);

        if (count == 0)
        {
            return Result<PaginatedList<BlogListResponse>>.Success(
                new PaginatedList<BlogListResponse>(
                    new List<BlogListResponse>(),
                    request.PageNumber,
                    0,
                    request.PageSize),
                _localizer["NoBlogsFound"]);
        }

        var items = await baseQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BlogListResponse
            {
                Id = b.Id,

                Title = new NameDto
                {
                    Ar = b.TitleAr,
                    En = b.TitleEn
                },

                Description = new DescriptionDto
                {
                    Ar = b.DescriptionAr,
                    En = b.DescriptionEn
                },

                Image = b.Image,

                IsActive = b.IsActive,

                Category = new LookupDto
                {
                    Id = b.Category.Id,

                    Name = new NameDto
                    {
                        Ar = b.Category.NameAr,
                        En = b.Category.NameEn
                    }
                },
                PublishingHouse = new LookupDto
                {
                    Id = b.PublishingHouse.Id,

                    Name = new NameDto
                    {
                        Ar = b.PublishingHouse.NameAr,
                        En = b.PublishingHouse.NameEn
                    }
                },

                Author = new LookupDto
                {
                    Id = b.Author.Id,

                    Name = new NameDto
                    {
                        Ar = b.Author.NameAr,
                        En = b.Author.NameEn
                    }
                },

                Tags = new TagsDto
                {
                    Ar = b.BlogTags
                        .Where(t => !string.IsNullOrWhiteSpace(t.Tag.NameAr))
                        .Select(t => t.Tag.NameAr!)
                        .ToList(),

                    En = b.BlogTags
                        .Where(t => !string.IsNullOrWhiteSpace(t.Tag.NameEn))
                        .Select(t => t.Tag.NameEn!)
                        .ToList()
                },

                CreatedAt = b.CreatedAt,

                CreatedBy = b.CreatedBy != null
                    ? b.CreatedBy.FullName
                    : string.Empty,

                UpdatedAt = b.UpdatedAt,

                UpdatedBy = b.UpdatedBy != null
                    ? b.UpdatedBy.FullName
                    : null,
                CreatedDate = b.CreatedDate ,
                CreatedTime = b .CreatedTime,
                Likes = b.Reactions.Count(r => r.Type == ReactionType.Like),

                Dislikes = b.Reactions.Count(r => r.Type == ReactionType.Dislike)
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedList<BlogListResponse>(
            items,
            request.PageNumber,
            count,
            request.PageSize);

        return Result<PaginatedList<BlogListResponse>>.Success(
            result,
            _localizer["BlogsRetrievedSuccessfully"]);
    }

    private IQueryable<Blog> BuildQuery(GetBlogsQuery request)
    {
        var query = _context.Blogs
            .AsNoTracking()
            .AsSplitQuery();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var search = $"%{request.Name.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.TitleEn ?? string.Empty, search) ||
                EF.Functions.Like(x.TitleAr ?? string.Empty, search));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        if (request.BlogCategoryId.HasValue)
        {
            query = query.Where(x =>
                x.BlogsCategoryId == request.BlogCategoryId.Value);
        }

        if (request.CreatedFrom.HasValue)
        {
            var createdFrom = request.CreatedFrom.Value.Date;

            query = query.Where(x =>
                x.CreatedAt >= createdFrom);
        }

        if (request.CreatedTo.HasValue)
        {
            var createdTo = request.CreatedTo.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.CreatedAt < createdTo);
        }

        return query.OrderByDescending(x => x.CreatedAt);
    }
}