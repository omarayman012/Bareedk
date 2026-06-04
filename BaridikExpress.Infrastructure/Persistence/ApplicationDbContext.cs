using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.ClientModule;
using BaridikExpress.Domain.Entities.ContactUs;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Entities.DeliveryType;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.Nationality;
using BaridikExpress.Domain.Entities.PublishingHouseModule;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Domain.Entities.Services;
using BaridikExpress.Domain.Entities.Shipments;
using BaridikExpress.Domain.Entities.SystemManagment;
using BaridikExpress.Domain.Entities.Vehicles;
using BaridikExpress.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaridikExpress.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly TimeZoneInfo _appTimeZone;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
        : base(options)
    {
        _httpContext = httpContextAccessor;
        _appTimeZone = ResolveAppTimeZone(configuration);
    }

    #region DbSets - Auth & Users

    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> ApplicationUsers { get; set; }
    public DbSet<SubAdminEmployee> SubAdminEmployees { get; set; }

    #endregion

    #region DbSets - Location

    public DbSet<Country> Countries { get; set; }
    public DbSet<Government> Governments { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Village> Villages { get; set; }
    public DbSet<Nationality> Nationalities { get; set; }

    #endregion

    #region DbSets - Customers

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAccount> CustomerAccounts { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }

    #endregion

    #region DbSets - Delivery

    public DbSet<Delivery> Deliveries { get; set; }
    DbSet<DeliveryType> IApplicationDbContext.DeliveryTypes { get => DeliveryTypes; set => DeliveryTypes = value; }
    DbSet<DeliveryType> DeliveryTypes { get; set; }

    #endregion

    #region DbSets - Shipments

    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentAddress> ShipmentAddresses { get; set; }
    public DbSet<ShipmentAttachment> ShipmentAttachments { get; set; }
    public DbSet<ShipmentService> ShipmentServices { get; set; }
    public DbSet<ShipmentStatusHistory> ShipmentStatusHistories { get; set; }

    #endregion

    #region DbSets - Core Business

    public DbSet<CareerField> CareerFields { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }
    public DbSet<PublishingHouse> PublishingHouses { get; set; }

    #endregion

    #region DbSets - System Management

    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
    public DbSet<PrivacyPolicy> PrivacyPolicies { get; set; }
    public DbSet<ShippingPolicy> ShippingPolicies { get; set; }
    public DbSet<SalesAndPurchasePolicy> SalesAndPurchasePolicies { get; set; }
    public DbSet<Help> Help { get; set; }
    public DbSet<DeliveryDriverRegistrationTerms> DeliveryDriverRegistrationTerms { get; set; }
    public DbSet<CustomerRegistration> CustomerRegistrationTerms { get; set; }
    public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
    public DbSet<FAQ> FAQs { get; set; }

    #endregion

    #region OnModelCreating

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (clrType == null) continue;

            if (!typeof(IAuditableEntity).IsAssignableFrom(clrType)
              || clrType == typeof(User)
              || clrType == typeof(BaseEntity))
                continue;

            modelBuilder.Entity(clrType)
                .HasOne(typeof(User), "CreatedBy")
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity(clrType)
                .HasOne(typeof(User), "UpdatedBy")
                .WithMany()
                .HasForeignKey("UpdatedById")
                .OnDelete(DeleteBehavior.NoAction);
        }

        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.User)
            .WithOne()
            .HasForeignKey<Delivery>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion

    #region SaveChangesAsync

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>();
        var currentUserId = _httpContext.HttpContext?.User?.GetUserId();
        var isAuthenticated = _httpContext.HttpContext?.User?.Identity?.IsAuthenticated == true;
        var now = GetAppNow();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedAt).CurrentValue = now;

                if (isAuthenticated && currentUserId != null)
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.CreatedById).IsModified = false;
                entityEntry.Property(x => x.CreatedAt).IsModified = false;
                entityEntry.Property(x => x.UpdatedAt).CurrentValue = now;

                if (isAuthenticated && currentUserId != null)
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Helpers

    private static TimeZoneInfo ResolveAppTimeZone(IConfiguration configuration)
    {
        var tzId =
            configuration["App:TimeZoneId"]
            ?? configuration["TimeZoneId"]
            ?? Environment.GetEnvironmentVariable("APP_TIME_ZONE_ID")
            ?? Environment.GetEnvironmentVariable("TZ")
            ?? "Egypt Standard Time";

        try { return TimeZoneInfo.FindSystemTimeZoneById(tzId); }
        catch { return TimeZoneInfo.Local; }
    }

    private DateTime GetAppNow() =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _appTimeZone);

    #endregion

}