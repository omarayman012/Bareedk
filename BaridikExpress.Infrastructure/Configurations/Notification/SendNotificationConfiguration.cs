using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Notification;

public class SendNotificationConfiguration : IEntityTypeConfiguration<SendNotification>
{
    public void Configure(EntityTypeBuilder<SendNotification> builder)
    {
        builder.ToTable("SendNotifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TitleAr)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.TitleEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DescriptionAr)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.DescriptionEn)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasMany(x => x.Recipients)
            .WithOne(x => x.Notification)
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}