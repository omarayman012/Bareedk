using BaridikExpress.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Customers;

internal sealed class CustomerContactConfiguration
    : IEntityTypeConfiguration<CustomerContact>
{
    public void Configure(EntityTypeBuilder<CustomerContact> builder)
    {
        builder.ToTable("CustomerContacts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneCountryCode)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.WhatsAppCountryCode)
            .HasMaxLength(10);

        builder.Property(x => x.WhatsAppNumber)
            .HasMaxLength(20);
    }
}