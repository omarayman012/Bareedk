using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Infrastructure.Configurations.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Location
{
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.ToTable("Countries");
            builder.HasKey(c => c.CountryId);

            builder.Property(c => c.CountryNameAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CountryNameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Governments)
                .WithOne(g => g.Country)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
