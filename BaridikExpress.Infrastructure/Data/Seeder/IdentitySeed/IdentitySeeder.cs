using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var context = services.GetRequiredService<ApplicationDbContext>();
            var scanner = services.GetRequiredService<IAutoPermissionScanner>(); 

            await RoleSeed.SeedAsync(roleManager);
            await PermissionSeed.SeedAsync(context, scanner);
            await RolePermissionSeed.SeedAsync(context);
            await UserSeed.SeedAsync(userManager);
        }
    }
}