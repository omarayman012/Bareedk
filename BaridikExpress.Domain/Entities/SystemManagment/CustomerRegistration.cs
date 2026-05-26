using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class CustomerRegistration : BaseEntity
{
    public Guid Id { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }

    private CustomerRegistration() { }

    public static CustomerRegistration Create(string? descriptionAr = null, string? descriptionEn = null)
    {
        return new CustomerRegistration
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
