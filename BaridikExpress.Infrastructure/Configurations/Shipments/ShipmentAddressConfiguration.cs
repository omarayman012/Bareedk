using BaridikExpress.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Persistence.Configurations.Shipments;

public class ShipmentAddressConfiguration : IEntityTypeConfiguration<ShipmentAddress>
{
    public void Configure(EntityTypeBuilder<ShipmentAddress> builder)
    {
        builder.ToTable("ShipmentAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Address)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.FloorNumber)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.ApartmentNumber)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.Landmark)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.Latitude)
            .HasColumnType("decimal(9,6)")
            .IsRequired();

        builder.Property(x => x.Longitude)
            .HasColumnType("decimal(9,6)")
            .IsRequired();

        // ── Relationships ────────────────────────────────────────────────────

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Government)
            .WithMany()
            .HasForeignKey(x => x.GovernmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Village)
            .WithMany()
            .HasForeignKey(x => x.VillageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}