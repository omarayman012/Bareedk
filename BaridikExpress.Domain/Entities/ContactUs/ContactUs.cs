using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.ContactUs;

public sealed class ContactUs : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }

    private ContactUs() { }

    public static ContactUs Create(
        string name,
        string email,
        string phone,
        string message)
    {
        return new ContactUs
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim(),
            Phone = phone.Trim(),
            Message = message.Trim(),
            IsRead = false,
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void MarkAsUnread()
    {
        IsRead = false;
    }
}