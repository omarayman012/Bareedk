using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Infrastructure.Configurations.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Location
{
    public class VillageConfiguration : BaseEntityConfiguration<Village>
    {
        public override void Configure(EntityTypeBuilder<Village> builder)
        {
            base.Configure(builder);

            builder.ToTable("Villages");
            builder.HasKey(v => v.VillageId);

            builder.Property(v => v.VillageNameAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.VillageNameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(v => v.City)
                .WithMany(c => c.Villages)
                .HasForeignKey(v => v.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
