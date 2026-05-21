using BaridikExpress.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Customers;

internal sealed class CustomerAccountConfiguration
    : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        builder.ToTable("CustomerAccounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TaxRegistrationNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Currency);

        builder.Property(x => x.OpeningBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Note)
            .HasMaxLength(1000);
    }
}