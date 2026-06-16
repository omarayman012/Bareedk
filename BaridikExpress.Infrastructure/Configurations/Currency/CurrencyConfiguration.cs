using BaridikExpress.Domain.Entities.CurrencyModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name, name =>
        {
            name.Property(x => x.En)
                .HasColumnName("NameEn")
                .HasMaxLength(100)
                .IsRequired();

            name.Property(x => x.Ar)
                .HasColumnName("NameAr")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(x => x.CurrencyCode)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.CurrencySymbol)
            .HasMaxLength(10);
    }
}