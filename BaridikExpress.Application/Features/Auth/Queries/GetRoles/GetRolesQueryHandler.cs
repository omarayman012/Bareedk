using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetRoles
{
    public class GetRolesQueryHandler(
        IApplicationDbContext context
    ) : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
    {
        public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await context.Roles
                .Select(r => new RoleDto(r.Id, r.Name!))
                .ToListAsync(cancellationToken);

            if (!roles.Any())
                return Result<List<RoleDto>>.Failure("No roles found", 404);

            return Result<List<RoleDto>>.Success(roles, "Operation completed successfully", 200);
        }
    }
}