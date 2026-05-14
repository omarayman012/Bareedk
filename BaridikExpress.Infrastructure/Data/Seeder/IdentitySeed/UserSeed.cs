using BaridikExpress.Domain.Entities.AuthModules;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed
{
    public static class UserSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var email = "admin@test.com";
            var password = "P@ssword123";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    FullName = "Admin",
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin,password);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
