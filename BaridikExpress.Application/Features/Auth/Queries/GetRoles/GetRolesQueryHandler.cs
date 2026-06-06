using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetRoles;

public class GetRolesQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer
) : IRequestHandler<GetRolesQuery, Result<PaginatedList<RoleDto>>>
{
    public async Task<Result<PaginatedList<RoleDto>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var rolesQuery = context.Roles
            .Where(r => string.IsNullOrEmpty(request.Name) ||
                        r.Name!.Contains(request.Name))
            .Select(r => new RoleDto(
                r.Id,
                r.Name!
            ));

        var roles = await PaginatedList<RoleDto>.CreateAsync(
            rolesQuery,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedList<RoleDto>>.Success(
            roles,
            localizer["RolesRetrievedSuccessfully"],
            200);
    }
}