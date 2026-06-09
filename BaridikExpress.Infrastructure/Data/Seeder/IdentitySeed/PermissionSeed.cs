using System.Reflection;
using BaridikExpress.Application.Common.Abstractions.Consts;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class PermissionSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            IAutoPermissionScanner scanner) 
        {
            var manualPermissions = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Select(x => x.GetValue(null)!.ToString()!)
                .ToList();

            var autoPermissions = scanner.ScanPermissions().ToList();

            var allPermissions = manualPermissions
                .Union(autoPermissions)
                .Distinct()
                .ToList();

            var existingDbPermissions = await context.Permissions
                .Select(p => p.PermissionName)
                .ToListAsync();

            var newPermissionsToSeed = allPermissions
                .Where(p => !existingDbPermissions.Contains(p))
                .Select(p => new Permission
                {
                    PermissionId = Guid.NewGuid(),
                    PermissionName = p
                }).ToList();

            if (newPermissionsToSeed.Any())
            {
                await context.Permissions.AddRangeAsync(newPermissionsToSeed);
                await context.SaveChangesAsync();
            }
        }
    }
}