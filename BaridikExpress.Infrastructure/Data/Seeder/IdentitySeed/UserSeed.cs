using BaridikExpress.Domain.Entities.AuthModules;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class UserSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            const string email = "baridikexpress@gmail.com";
            const string password = "J+TVosZIruHX+p0C";
            const string role = "SuperAdmin";

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return;

            var admin = new User
            {
                UserName = "baridikexpress",
                FullName = "baridikexpress",
                Email = email,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await userManager.CreateAsync(admin, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create seed user: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(admin, role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role '{role}': {errors}");
            }
        }
    }
}