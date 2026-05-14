using BaridikExpress.Domain.Entities.AuthModules;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class UserSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            await SeedUserAsync(userManager,
                email: "baridikexpress@gmail.com",
                password: "J+TVosZIruHX+p0C",
                userName: "baridikexpress",
                fullName: "baridikexpress",
                role: "SuperAdmin");

            await SeedUserAsync(userManager,
                email: "client@gmail.com",
                password: "Client@2026",
                userName: "client",
                fullName: "client",
                role: "Client");

            await SeedUserAsync(userManager,
                email: "delivery@gmail.com",
                password: "Delivery@2026",
                userName: "delivery",
                fullName: "delivery",
                role: "Delivery");
        }

        private static async Task SeedUserAsync(
            UserManager<User> userManager,
            string email,
            string password,
            string userName,
            string fullName,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null) return;

            var user = new User
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create seed user '{email}': {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role '{role}' to '{email}': {errors}");
            }
        }
    }
}