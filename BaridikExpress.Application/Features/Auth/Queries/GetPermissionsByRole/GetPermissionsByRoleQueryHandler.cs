// Handler
using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetPermissionsByRole
{
    public class GetPermissionsByRoleQueryHandler(
        IApplicationDbContext context
    ) : IRequestHandler<GetPermissionsByRoleQuery, Result<List<PermissionDto>>>
    {
        public async Task<Result<List<PermissionDto>>> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
        {
            var roleExists = await context.Roles
                .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

            if (!roleExists)
                return Result<List<PermissionDto>>.Failure("Role not found", 404);

            var permissions = await context.RolePermissions
                .Where(rp => rp.RoleId == request.RoleId)
                .Select(rp => new PermissionDto(
                    rp.PermissionId,
                    rp.Permission.PermissionName
                ))
                .ToListAsync(cancellationToken);

            if (!permissions.Any())
                return Result<List<PermissionDto>>.Failure("No permissions found for this role", 404);

            return Result<List<PermissionDto>>.Success(permissions, "Operation completed successfully", 200);
        }
    }
}