using BaridikExpress.Domain.Entities.DeliveryType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.DeliveryTypes;

public class DeliveryTypeConfiguration : IEntityTypeConfiguration<DeliveryType>
{
    public void Configure(EntityTypeBuilder<DeliveryType> builder)
    {
        builder.ToTable("DeliveryTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.NameEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.NameEn)
            .IsUnique();

        builder.Property(x => x.NameAr)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.NameAr)
            .IsUnique();

        builder.Property(x => x.DaysFrom)
            .IsRequired();

        builder.Property(x => x.DaysTo)
            .IsRequired();

        builder.Property(x => x.PricePerShipment)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IsSwitchActive)
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.DescriptionEn)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.DescriptionAr)
            .HasMaxLength(1000)
            .IsRequired(false);
        builder.Property(x => x.Currency)
    .IsRequired();

        builder.Ignore(x => x.PricePerTotal);
    }
}