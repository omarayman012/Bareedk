using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;
public class SocialMediaLinks : BaseEntity
{
    public Guid Id { get; private set; }
    public string PlatformName { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;

    private SocialMediaLinks() { }

    public static SocialMediaLinks Create(string platformName, string url)
    {
        return new SocialMediaLinks
        {
            Id = Guid.NewGuid(),
            PlatformName = platformName,
            Url = url,
        };
    }

    public void Update(string? platformName = null, string? url = null)
    {
        if (!string.IsNullOrWhiteSpace(platformName)) PlatformName = platformName;
        if (!string.IsNullOrWhiteSpace(url)) Url = url;
    }
}