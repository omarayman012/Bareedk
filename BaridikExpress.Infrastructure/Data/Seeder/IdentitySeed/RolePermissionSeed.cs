using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class RolePermissionSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var superAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

            if (superAdminRole is null) return;

            var permissions = await context.Permissions.ToListAsync();

            var existingPermissionIds = await context.RolePermissions
                .Where(rp => rp.RoleId == superAdminRole.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var newRolePermissions = permissions
                .Where(p => !existingPermissionIds.Contains(p.PermissionId))
                .Select(p => new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = superAdminRole.Id,
                    PermissionId = p.PermissionId
                })
                .ToList();

            if (!newRolePermissions.Any()) return;

            await context.RolePermissions.AddRangeAsync(newRolePermissions);
            await context.SaveChangesAsync();
        }
    }
}