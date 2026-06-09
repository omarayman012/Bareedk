using BaridikExpress.Application.Features.SelectMenu.Queries.GetRolesSelectMenu;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetRolesSelectMenu;

public sealed class GetRolesSelectMenuQueryHandler(
    RoleManager<IdentityRole> roleManager,
    IStringLocalizer localizer)
    : IRequestHandler<GetRolesSelectMenuQuery, Result<IEnumerable<RoleSelectMenuResponse>>>
{
    #region Handle

    public async Task<Result<IEnumerable<RoleSelectMenuResponse>>> Handle(
        GetRolesSelectMenuQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new RoleSelectMenuResponse(x.Id, x.Name!))
            .ToListAsync(cancellationToken);

        if (!roles.Any())
            return Result<IEnumerable<RoleSelectMenuResponse>>
                .Failure(localizer["RolesNotFound"], 404);

        return Result<IEnumerable<RoleSelectMenuResponse>>
            .Success(roles, localizer["RolesRetrievedSuccessfully"]);
    }

    #endregion
}