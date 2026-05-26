using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;
public class DeliveryDriverRegistrationTerms : BaseEntity
{
    public Guid Id { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }

    private DeliveryDriverRegistrationTerms() { }

    public static DeliveryDriverRegistrationTerms Create(string? descriptionAr = null, string? descriptionEn = null)
    {
        return new DeliveryDriverRegistrationTerms
        {
            Id = Guid.NewGuid(),
            DescriptionAr = descriptionAr,
            DescriptionEn = descriptionEn,
        };
    }

    public void Update(string? descriptionAr = null, string? descriptionEn = null)
    {
        if (!string.IsNullOrWhiteSpace(descriptionAr)) DescriptionAr = descriptionAr;
        if (!string.IsNullOrWhiteSpace(descriptionEn)) DescriptionEn = descriptionEn;
    }
}