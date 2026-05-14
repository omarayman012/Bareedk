using BaridikExpress.Domain.Entities.RoleModule;

namespace BaridikExpress.Application.Features.Auth.Commands.UpdateRolePermissions
{
    public class UpdateRolePermissionsCommandHandler(
        IApplicationDbContext context
    ) : IRequestHandler<UpdateRolePermissionsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var roleExists = await context.Roles
                .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

            if (!roleExists)
                return Result<string>.Failure("Role not found", 404);

            var validPermissionIds = await context.Permissions
                .Where(p => request.PermissionIds.Contains(p.PermissionId))
                .Select(p => p.PermissionId)
                .ToListAsync(cancellationToken);

            var invalidIds = request.PermissionIds.Except(validPermissionIds).ToList();
            if (invalidIds.Any())
                return Result<string>.Failure($"Invalid permission IDs: {string.Join(", ", invalidIds)}", 400);

            var existingPermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == request.RoleId)
                .ToListAsync(cancellationToken);

            context.RolePermissions.RemoveRange(existingPermissions);

            var newPermissions = request.PermissionIds
                .Select(permId => new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = request.RoleId,
                    PermissionId = permId
                }).ToList();

            await context.RolePermissions.AddRangeAsync(newPermissions, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Role permissions updated successfully", "Operation completed successfully", 200);
        }
    }
}