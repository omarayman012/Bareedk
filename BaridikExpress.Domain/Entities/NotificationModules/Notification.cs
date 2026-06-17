using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.NotificationModules;

public class Notification : BaseEntity
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public Guid? SendNotificationId { get; set; }
    public SendNotification? SendNotification { get; set; }

    public string TitleAr { get; set; } = default!;
    public string TitleEn { get; set; } = default!;

    public string MessageAr { get; set; } = default!;
    public string MessageEn { get; set; } = default!;

    public string? ImageUrl { get; set; }

    public bool IsRead { get; set; } = false;

    public Guid? BlogId { get; set; }
    public Guid? CommentId { get; set; }

    public static Notification Create(
        string userId,
        string titleAr,
        string titleEn,
        string messageAr,
        string messageEn,
        string? imageUrl = null,
        Guid? sendNotificationId = null,
        Guid? blogId = null,
        Guid? commentId = null)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TitleAr = titleAr,
            TitleEn = titleEn,
            MessageAr = messageAr,
            MessageEn = messageEn,
            ImageUrl = imageUrl,
            SendNotificationId = sendNotificationId,
            BlogId = blogId,
            CommentId = commentId,
            IsRead = false
        };
    }
}