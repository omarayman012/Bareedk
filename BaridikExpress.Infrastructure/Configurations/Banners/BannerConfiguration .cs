
using BaridikExpress.Domain.Entities.Banners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BaridikExpress.Infrastructure.Configurations.Banners
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("Banners");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.TitleEn)
                .IsRequired(false);

            builder.Property(x => x.TitleAr)
                .IsRequired(false);

            builder.Property(x => x.DescriptionEn)
                .IsRequired(false);

            builder.Property(x => x.DescriptionAr)
                .IsRequired(false);
            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
