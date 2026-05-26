using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class PrivacyPolicy : BaseEntity
{
    public Guid Id { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }

    private PrivacyPolicy() { }

    public static PrivacyPolicy Create(
        string? descriptionAr = null,
        string? descriptionEn = null)
    {
        return new PrivacyPolicy
        {
            Id = Guid.NewGuid(),
            DescriptionAr = descriptionAr,
            DescriptionEn = descriptionEn,
        };
    }

    public void Update(string? descriptionAr = null, string? descriptionEn = null)
    {
        if (descriptionAr is not null) DescriptionAr = descriptionAr == string.Empty ? null : descriptionAr.Trim();
        if (descriptionEn is not null) DescriptionEn = descriptionEn == string.Empty ? null : descriptionEn.Trim();
    }
}