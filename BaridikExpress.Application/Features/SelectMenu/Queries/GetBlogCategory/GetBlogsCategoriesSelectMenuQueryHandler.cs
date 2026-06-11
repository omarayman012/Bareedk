using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetBlogCategory;

public sealed class GetBlogsCategoriesSelectMenuQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBlogsCategoriesSelectMenuQuery, Result<List<SelectMenuResponse>>>
{
    public async Task<Result<List<SelectMenuResponse>>> Handle(
        GetBlogsCategoriesSelectMenuQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.BlogsCategorys.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var s = request.Name.Trim().ToLower();
            query = query.Where(x =>
                (x.NameEn ?? string.Empty).ToLower().Contains(s) ||
                (x.NameAr ?? string.Empty).ToLower().Contains(s));
        }

        var list = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SelectMenuResponse(
                x.Id,
                new LocalizeLang
                {
                    EN = x.NameEn ?? string.Empty,
                    AR = x.NameAr ?? string.Empty
                }
            ))
            .ToListAsync(cancellationToken);

        return Result<List<SelectMenuResponse>>.Success(
            list,
            "Data fetched successfully.",
            200);
    }
}