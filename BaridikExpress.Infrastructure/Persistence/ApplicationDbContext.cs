using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.RoleModule;
using BaridikExpress.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaridikExpress.Infrastructure.Persistence
{
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

        // DbSets
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CareerField> CareerFields { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (clrType == null)
                    continue;

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
             
                modelBuilder.Entity<Delivery>()
                    .HasOne(d => d.User)
                    .WithOne()
                    .HasForeignKey<Delivery>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

            modelBuilder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

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
        private static TimeZoneInfo ResolveAppTimeZone(IConfiguration configuration)
        {
            var tzId =
                configuration["App:TimeZoneId"]
                ?? configuration["TimeZoneId"]
                ?? Environment.GetEnvironmentVariable("APP_TIME_ZONE_ID")
                ?? Environment.GetEnvironmentVariable("TZ")
                ?? "Egypt Standard Time";

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(tzId);
            }
            catch
            {
                return TimeZoneInfo.Local;
            }
        }

        private DateTime GetAppNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _appTimeZone);
        }
    }
}