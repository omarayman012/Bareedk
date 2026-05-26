using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class Help : BaseEntity
{
    public Guid Id { get; private set; }
    public string? DescriptionAr { get; private set; }
    public string? DescriptionEn { get; private set; }

    private Help() { }

    public static Help Create(string? descriptionAr = null, string? descriptionEn = null)
    {
        return new Help
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
