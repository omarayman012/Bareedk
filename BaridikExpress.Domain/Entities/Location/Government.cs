using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class Government : BaseEntity, ISelectMenuEntity
{
    public Guid GovernmentId { get; set; }
    public string GovernmentNameAr { get; set; } = string.Empty;
    public string GovernmentNameEn { get; set; } = string.Empty;
    public string? NameAr => GovernmentNameAr;
    public string? NameEn => GovernmentNameEn;
    public Guid Id => GovernmentId;
    public Guid? ParentId => CountryId;
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public ICollection<City>? Cities { get; set; }
}