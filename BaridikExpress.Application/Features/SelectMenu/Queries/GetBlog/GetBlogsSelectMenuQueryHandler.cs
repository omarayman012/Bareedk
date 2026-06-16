using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetBlogCategory;

namespace BaridikExpress.Application.Features.Blogs.Queries.SelectMenu;

public sealed class GetBlogsSelectMenuQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBlogsSelectMenuQuery, Result<List<SelectMenuResponse>>>
{
    public async Task<Result<List<SelectMenuResponse>>> Handle(
        GetBlogsSelectMenuQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Blogs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var s = request.Name.Trim().ToLower();
            query = query.Where(x =>
                (x.TitleEn ?? string.Empty).ToLower().Contains(s) ||
                (x.TitleAr ?? string.Empty).ToLower().Contains(s));
        }

        var list = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SelectMenuResponse(
                x.Id,
                new LocalizeLang
                {
                    EN = x.TitleEn ?? string.Empty,
                    AR = x.TitleAr ?? string.Empty
                }
            ))
            .ToListAsync(cancellationToken);

        return Result<List<SelectMenuResponse>>.Success(
            list,
            "Data fetched successfully.",
            200);
    }

    public sealed class GetBlogsAuthorsSelectMenuQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBlogsAuthorsSelectMenuQuery, Result<List<SelectMenuResponse>>>
    {
        public async Task<Result<List<SelectMenuResponse>>> Handle(
            GetBlogsAuthorsSelectMenuQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.BlogsAuthors
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim().ToLower();

                query = query.Where(x =>
                    (x.NameAr ?? string.Empty).ToLower().Contains(search) ||
                    (x.NameEn ?? string.Empty).ToLower().Contains(search));
            }

            var list = await query
                .OrderBy(x => x.NameEn)
                .Select(x => new SelectMenuResponse(
                    x.Id,
                    new LocalizeLang
                    {
                        AR = x.NameAr ?? string.Empty,
                        EN = x.NameEn ?? string.Empty
                    }))
                .ToListAsync(cancellationToken);

            return Result<List<SelectMenuResponse>>.Success(
                list,
                "Data fetched successfully.",
                200);
        }
    }
    public sealed class GetTagsSelectMenuQueryHandler(
    IApplicationDbContext context)
    : IRequestHandler<GetTagsSelectMenuQuery, Result<List<SelectMenuResponse>>>
    {
        public async Task<Result<List<SelectMenuResponse>>> Handle(
            GetTagsSelectMenuQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.Tags.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim().ToLower();

                query = query.Where(x =>
                    (x.NameAr ?? string.Empty).ToLower().Contains(search) ||
                    (x.NameEn ?? string.Empty).ToLower().Contains(search));
            }

            var list = await query
                .OrderBy(x => x.NameEn)
                .Select(x => new SelectMenuResponse(
                    x.Id,
                    new LocalizeLang
                    {
                        AR = x.NameAr ?? string.Empty,
                        EN = x.NameEn ?? string.Empty
                    }))
                .ToListAsync(cancellationToken);

            return Result<List<SelectMenuResponse>>.Success(
                list,
                "Data fetched successfully.",
                200);
        }
    }
    public sealed class GetPublishingHousesSelectMenuQueryHandler(
    IApplicationDbContext context)
    : IRequestHandler<GetPublishingHousesSelectMenuQuery, Result<List<SelectMenuResponse>>>
    {
        public async Task<Result<List<SelectMenuResponse>>> Handle(
            GetPublishingHousesSelectMenuQuery request,
            CancellationToken cancellationToken)
        {
            var query = context.PublishingHouses
                .AsNoTracking()
                .Where(x => x.Active);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim().ToLower();

                query = query.Where(x =>
                    (x.NameAr ?? string.Empty).ToLower().Contains(search) ||
                    (x.NameEn ?? string.Empty).ToLower().Contains(search) ||
                    (x.Code ?? string.Empty).ToLower().Contains(search));
            }

            var list = await query
                .OrderBy(x => x.NameEn)
                .Select(x => new SelectMenuResponse(
                    x.Id,
                    new LocalizeLang
                    {
                        AR = x.NameAr ?? string.Empty,
                        EN = x.NameEn ?? string.Empty
                    }))
                .ToListAsync(cancellationToken);

            return Result<List<SelectMenuResponse>>.Success(
                list,
                "Data fetched successfully.",
                200);
        }
    }
}