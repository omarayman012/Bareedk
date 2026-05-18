using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetRoles;

public class GetRolesQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer
) : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    public async Task<Result<List<RoleDto>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await context.Roles
            .Select(r => new RoleDto(r.Id, r.Name!))
            .ToListAsync(cancellationToken);

        return Result<List<RoleDto>>.Success(
            roles,
            localizer["RolesRetrievedSuccessfully"],
            200);
    }
}