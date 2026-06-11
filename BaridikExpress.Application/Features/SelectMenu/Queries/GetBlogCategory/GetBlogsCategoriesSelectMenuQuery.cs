using BaridikExpress.Application.Features.SelectMenu.DTOs;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetBlogCategory;

public sealed record GetBlogsCategoriesSelectMenuQuery(string? Name)
    : IRequest<Result<List<SelectMenuResponse>>>;