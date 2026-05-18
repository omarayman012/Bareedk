using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Infrastructure.Configurations.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Location
{
    public class CityConfiguration : BaseEntityConfiguration<City>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            base.Configure(builder);

            builder.ToTable("Cities");
            builder.HasKey(c => c.CityId);

            builder.Property(c => c.CityNameAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CityNameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.Government)
                .WithMany(g => g.Cities)
                .HasForeignKey(c => c.GovernmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Villages)
                .WithOne(v => v.City)
                .HasForeignKey(v => v.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
