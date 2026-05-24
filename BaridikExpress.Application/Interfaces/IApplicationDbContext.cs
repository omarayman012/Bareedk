using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.ClientModule;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Entities.DeliveryType;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.Nationality;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        DbSet<Delivery> Deliveries { get; }

        DbSet<CareerField> CareerFields { get; set; }
        DbSet<Vehicle>  Vehicles { get; set; }
        DbSet<Customer> Customers { get; set; }
         DbSet<CustomerAccount> CustomerAccounts { get; set; }
         DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Government> Governments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<SubAdminEmployee> SubAdminEmployees { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Client> Clients { get; set; }

        DatabaseFacade Database { get; }
        DbSet<DeliveryType> DeliveryTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}