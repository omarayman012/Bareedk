using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.SystemManagement;

public class AboutUsConfiguration : IEntityTypeConfiguration<AboutUs>
{
    public void Configure(EntityTypeBuilder<AboutUs> builder)
    {
        builder.ToTable("AboutUs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.TitleAr).HasMaxLength(300).IsRequired(false);
        builder.Property(x => x.TitleEn).HasMaxLength(300).IsRequired(false);
        builder.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired(false);
        builder.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired(false);
        builder.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.VideoUrl).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.ExternalLinkYoutube).HasMaxLength(500).IsRequired(false);
    }
}