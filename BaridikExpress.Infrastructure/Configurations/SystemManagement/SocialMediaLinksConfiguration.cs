using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.SystemManagement;

public class SocialMediaLinksConfiguration : IEntityTypeConfiguration<SocialMediaLinks>
{
    public void Configure(EntityTypeBuilder<SocialMediaLinks> builder)
    {
        builder.ToTable("SocialMediaLinks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.PlatformName).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.PlatformName).IsUnique();

        builder.Property(x => x.Url).HasMaxLength(500).IsRequired();
    }
}