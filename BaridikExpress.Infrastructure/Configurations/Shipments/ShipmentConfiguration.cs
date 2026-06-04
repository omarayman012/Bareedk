using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Persistence.Configurations.Shipments;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        #region Properties

        builder.Property(x => x.TrackingId)
            .HasMaxLength(50)
            .IsRequired(false);
        builder.HasIndex(x => x.TrackingId)
            .IsUnique();

        builder.Property(x => x.TotalWeight)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.NumberOfPieces)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.ExpectedSendingDate)
            .IsRequired(false);

        builder.Property(x => x.HasDimensions)
            .HasDefaultValue(false);

        #endregion

        #region Relationships

        builder.HasOne<User>(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SenderAddress)
            .WithMany()
            .HasForeignKey(x => x.SenderAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReceiverAddress)
            .WithMany()
            .HasForeignKey(x => x.ReceiverAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Vehicle)
            .WithMany()
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DeliveryType)
            .WithMany()
            .HasForeignKey(x => x.DeliveryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.Shipment)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ShipmentServices)
            .WithOne(x => x.Shipment)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.StatusHistory)
            .WithOne(x => x.Shipment)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}