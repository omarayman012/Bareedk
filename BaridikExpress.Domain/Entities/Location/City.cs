using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class City : BaseEntity, ISelectMenuEntity
{
    public Guid CityId { get; set; }

    public string CityNameAr { get; set; } = string.Empty;
    public string CityNameEn { get; set; } = string.Empty;

    public string? NameAr => CityNameAr;
    public string? NameEn => CityNameEn;
    public Guid Id => CityId;
    public Guid? ParentId => GovernmentId;

    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public Guid GovernmentId { get; set; }
    public Government? Government { get; set; }

    public ICollection<Village>? Villages { get; set; }
}