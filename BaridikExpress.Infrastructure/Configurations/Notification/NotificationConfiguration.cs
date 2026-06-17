using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Notification;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Domain.Entities.NotificationModules.Notification>
{
    public void Configure(
        EntityTypeBuilder<Domain.Entities.NotificationModules.Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.SendNotificationId)
            .IsRequired(false);

        builder.Property(x => x.TitleAr)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.TitleEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.MessageAr)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.MessageEn)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsRead)
            .IsRequired();

        builder.Property(x => x.BlogId)
            .IsRequired(false);

        builder.Property(x => x.CommentId)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SendNotification)
            .WithMany()
            .HasForeignKey(x => x.SendNotificationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.SendNotificationId);

        builder.HasIndex(x => new
        {
            x.UserId,
            x.IsRead
        });
    }
}