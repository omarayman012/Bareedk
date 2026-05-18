using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Infrastructure.Configurations.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Location
{
    public class GovernmentConfiguration : BaseEntityConfiguration<Government>
    {
        public override void Configure(EntityTypeBuilder<Government> builder)
        {
            base.Configure(builder);

            builder.ToTable("Governments");
            builder.HasKey(g => g.GovernmentId);

            builder.Property(g => g.GovernmentNameAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.GovernmentNameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(g => g.Country)
                .WithMany(c => c.Governments)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(g => g.Cities)
                .WithOne(c => c.Government)
                .HasForeignKey(c => c.GovernmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
