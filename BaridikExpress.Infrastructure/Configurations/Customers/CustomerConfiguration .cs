using BaridikExpress.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Customers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.Mobile)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.WhatsappNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Nationality)
                .HasMaxLength(100);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.CustomerImage)
                .HasMaxLength(1000);

            builder.Property(x => x.TaxRegistrationNumber)
                .HasMaxLength(100);

            builder.Property(x => x.OpeningBalance)
                .HasPrecision(18, 2);

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);



            builder.OwnsOne(x => x.Name, b =>
            {
                b.Property(p => p.En)
                    .HasColumnName("NameEn")
                    .HasMaxLength(300)
                    .IsRequired();

                b.Property(p => p.Ar)
                    .HasColumnName("NameAr")
                    .HasMaxLength(300)
                    .IsRequired();
                b.HasIndex(p => p.En);
                b.HasIndex(p => p.Ar);

            });

        }
    }
}