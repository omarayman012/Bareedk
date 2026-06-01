using BaridikExpress.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Persistence.Configurations.Shipments;

public class ShipmentServiceConfiguration : IEntityTypeConfiguration<ShipmentService>
{
    public void Configure(EntityTypeBuilder<ShipmentService> builder)
    {
        builder.ToTable("ShipmentServices");

        // Composite PK — no surrogate key needed
        builder.HasKey(x => new { x.ShipmentId, x.ServiceId });

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // TotalPrice is computed — not stored in DB
        builder.Ignore(x => x.TotalPrice);

        builder.HasOne(x => x.Shipment)
            .WithMany(x => x.ShipmentServices)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}