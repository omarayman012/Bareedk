using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Notification;

public class MessageNotificationConfiguration : IEntityTypeConfiguration<MessageNotification>
{
    void IEntityTypeConfiguration<MessageNotification>.Configure(EntityTypeBuilder<MessageNotification> builder)
    {
        builder.ToTable("MessageNotifications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired(false);
        builder.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired(false);
    }
}
