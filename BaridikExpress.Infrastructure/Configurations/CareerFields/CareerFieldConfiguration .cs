using BaridikExpress.Domain.Entities.CareerFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.CareerFields
{
    public class CareerFieldConfiguration : IEntityTypeConfiguration<CareerField>
    {
        public void Configure(EntityTypeBuilder<CareerField> builder)
        {
            builder.ToTable("CareerFields");

            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Name, b =>
            {
                b.Property(p => p.En)
                    .HasColumnName("NameEn")
                    .HasMaxLength(200)
                    .IsRequired();

                b.Property(p => p.Ar)
                    .HasColumnName("NameAr")
                    .HasMaxLength(200)
                    .IsRequired();
                b.HasIndex(p => p.En);
                b.HasIndex(p => p.Ar);
            });


            builder.HasMany(x => x.Customers)
                .WithOne(x => x.CareerField)
                .HasForeignKey(x => x.CareerFieldId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}