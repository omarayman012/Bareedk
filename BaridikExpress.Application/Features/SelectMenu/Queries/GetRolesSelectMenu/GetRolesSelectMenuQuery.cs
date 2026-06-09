namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetRolesSelectMenu;

public sealed record GetRolesSelectMenuQuery
    : IRequest<Result<IEnumerable<RoleSelectMenuResponse>>>;

public sealed record RoleSelectMenuResponse(
    string Id,
    string Name);