using BaridikExpress.Application.Common.Abstractions.Consts;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class RolePermissionSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            if (context.RolePermissions.Any()) return;

            var adminRole = await roleManager.FindByNameAsync("Admin");
            var userRole = await roleManager.FindByNameAsync("User");

            var permissions = context.Permissions.ToList();

           
            foreach (var perm in permissions)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = adminRole.Id,
                    PermissionId = perm.PermissionId
                });
            }

           
            var userPerms = permissions.Where(p =>
                p.PermissionName == Permissions.UsersRead ||
                p.PermissionName == Permissions.AuthView
            );

            foreach (var perm in userPerms)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = userRole.Id,
                    PermissionId = perm.PermissionId
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
