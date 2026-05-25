using BaridikExpress.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Services;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

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

        builder.Property(x => x.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ImageUrl)
               .HasMaxLength(500)
               .IsRequired(false);
    }
}