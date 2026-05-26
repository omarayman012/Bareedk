using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class AboutUs : BaseEntity
{
    public Guid Id { get; private set; }
    public string? TitleAr { get; private set; }
    public string? TitleEn { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }
    public string ?ImageUrl { get; private set; } 
    public string? VideoUrl { get; private set; }
    public string? ExternalLinkYoutube { get; private set; }

    private AboutUs() { }

    public static AboutUs Create(
        string? imageUrl = null,
        string? titleAr = null,
        string? titleEn = null,
        string? descriptionAr = null,
        string? descriptionEn = null,
        string? videoUrl = null,
        string? externalLinkYoutube = null)
    {
        return new AboutUs
        {
            Id = Guid.NewGuid(),
            TitleAr = titleAr,
            TitleEn = titleEn,
            DescriptionAr = descriptionAr,
            DescriptionEn = descriptionEn,
            ImageUrl = imageUrl,
            VideoUrl = videoUrl,
            ExternalLinkYoutube = externalLinkYoutube,
        };
    }

    public void Update(
        string? titleAr = null,
        string? titleEn = null,
        string? descriptionAr = null,
        string? descriptionEn = null,
        string? imageUrl = null,
        string? videoUrl = null,
        string? externalLinkYoutube = null)
    {
        if (titleAr is not null) TitleAr = titleAr == string.Empty ? null : titleAr.Trim();
        if (titleEn is not null) TitleEn = titleEn == string.Empty ? null : titleEn.Trim();
        if (descriptionAr is not null) DescriptionAr = descriptionAr == string.Empty ? null : descriptionAr.Trim();
        if (descriptionEn is not null) DescriptionEn = descriptionEn == string.Empty ? null : descriptionEn.Trim();
        if (!string.IsNullOrWhiteSpace(imageUrl)) ImageUrl = imageUrl;
        if (videoUrl is not null) VideoUrl = videoUrl == string.Empty ? null : videoUrl.Trim();
        if (externalLinkYoutube is not null) ExternalLinkYoutube = externalLinkYoutube == string.Empty ? null : externalLinkYoutube.Trim();
    }
}