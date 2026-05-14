using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.RoleModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Interfaces
{
    public interface IApplicationDbContext
    {
       
        DbSet<User> ApplicationUsers { get; }
        DbSet<IdentityRole> Roles { get; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        DbSet<IdentityUserRole<string>> UserRoles { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<CareerField> CareerFields { get; set; }
        DbSet<Customer> Customers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}