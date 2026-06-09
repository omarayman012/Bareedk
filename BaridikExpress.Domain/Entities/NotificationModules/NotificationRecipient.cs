using BaridikExpress.Domain.Entities.AuthModules;

namespace BaridikExpress.Domain.Entities.NotificationModules;

public sealed class NotificationRecipient
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public SendNotification Notification { get; set; } = default!;
    public User User { get; set; } = default!;

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }
}