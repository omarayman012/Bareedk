
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Vehicles
{
    namespace BaridikExpress.Infrastructure.Persistence.Configurations.Vehicles
    {
        public class VehicleConfigurations
            : IEntityTypeConfiguration<Vehicle>
        {
            public void Configure(EntityTypeBuilder<Vehicle> builder)
            {
                builder.ToTable("Vehicles");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.NameEn)
                    .IsRequired()
                    .HasMaxLength(150);
                builder.Property(x => x.NameAr)
                    .IsRequired()
                    .HasMaxLength(150);
                builder.Property(x => x.LoadCapacityFrom)
                    .HasColumnType("decimal(18,2)");

                builder.Property(x => x.LoadCapacityTo)
                    .HasColumnType("decimal(18,2)");

                builder.Property(x => x.PricePerTon)
                    .HasColumnType("decimal(18,2)");

                builder.Property(x => x.ImageUrl)
                    .HasMaxLength(500)
                    .IsRequired();

                builder.Property(x => x.IsPriceCalculationEnabled)
                    .HasDefaultValue(false);

                builder.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                builder.HasIndex(x => new { x.NameEn, x.NameAr })
                    .IsUnique();
                builder.HasIndex(x=>x.PricePerTon);
            }
        }
    }
}
