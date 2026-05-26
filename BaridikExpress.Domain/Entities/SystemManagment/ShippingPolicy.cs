using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class ShippingPolicy : BaseEntity
{
    public Guid Id { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }

    private ShippingPolicy() { }

    public static ShippingPolicy Create(
        string? descriptionAr = null,
        string? descriptionEn = null)
    {
        return new ShippingPolicy
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
