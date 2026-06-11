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
}