using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class RoleSeed
    {
        private static readonly string[] Roles = { "SuperAdmin", "Client", "Delivery" };
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (await roleManager.RoleExistsAsync(role)) continue;

                var result = await roleManager.CreateAsync(new IdentityRole(role)
                {
                    NormalizedName = role.ToUpperInvariant()
                });

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role '{role}': {errors}");
                }
            }
        }
    }
}