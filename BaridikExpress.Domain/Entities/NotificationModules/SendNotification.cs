// SendNotification.cs
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.NotificationModules;

public sealed class SendNotification : BaseEntity
{
    public Guid Id { get; private set; }
    public string TitleAr { get; private set; } = string.Empty;
    public string TitleEn { get; private set; } = string.Empty;
    public string DescriptionAr { get; private set; } = string.Empty;
    public string DescriptionEn { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }

    private readonly List<NotificationRecipient> _recipients = [];
    public IReadOnlyCollection<NotificationRecipient> Recipients => _recipients;

    private SendNotification() { }

    public static SendNotification Create(
        string titleAr,
        string titleEn,
        string descriptionAr,
        string descriptionEn,
        List<string> userIds,
        string? imageUrl = null)
    {
        var notification = new SendNotification
        {
            Id = Guid.NewGuid(),
            TitleAr = titleAr,
            TitleEn = titleEn,
            DescriptionAr = descriptionAr,
            DescriptionEn = descriptionEn,
            ImageUrl = imageUrl,
        };

        foreach (var userId in userIds)
        {
            notification._recipients.Add(new NotificationRecipient
            {
                Id = Guid.NewGuid(),
                NotificationId = notification.Id,
                UserId = userId,
                IsRead = false,
            });
        }

        return notification;
    }

    public void Update(
        string? titleAr = null,
        string? titleEn = null,
        string? descriptionAr = null,
        string? descriptionEn = null,
        string? imageUrl = null)
    {
        if (!string.IsNullOrWhiteSpace(titleAr)) TitleAr = titleAr;
        if (!string.IsNullOrWhiteSpace(titleEn)) TitleEn = titleEn;
        if (!string.IsNullOrWhiteSpace(descriptionAr)) DescriptionAr = descriptionAr;
        if (!string.IsNullOrWhiteSpace(descriptionEn)) DescriptionEn = descriptionEn;
        if (!string.IsNullOrWhiteSpace(imageUrl)) ImageUrl = imageUrl;
    }
}