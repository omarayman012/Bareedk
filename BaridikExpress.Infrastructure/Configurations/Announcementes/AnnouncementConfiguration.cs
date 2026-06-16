using global::BaridikExpress.Domain.Entities.Announcementes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Announcementes
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.TextColor)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.BackgroundColor)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
