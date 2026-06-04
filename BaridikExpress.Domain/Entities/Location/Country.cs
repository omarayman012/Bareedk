using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class Country : BaseEntity, ISelectMenuEntity
{
    public Guid CountryId { get; set; }
    public string CountryNameAr { get; set; } = string.Empty;
    public string CountryNameEn { get; set; } = string.Empty;
    public string? NameAr => CountryNameAr;
    public string? NameEn => CountryNameEn;
    public Guid? ParentId => null;

    public Guid Id => CountryId;
    public ICollection<Government>? Governments { get; set; }
}