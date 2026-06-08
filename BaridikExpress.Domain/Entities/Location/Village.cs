using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class Village : BaseEntity, ISelectMenuEntity
{
    public Guid VillageId { get; set; }

    public string VillageNameAr { get; set; } = string.Empty;
    public string VillageNameEn { get; set; } = string.Empty;

    public string? NameAr => VillageNameAr;
    public string? NameEn => VillageNameEn;
    public Guid Id => VillageId;  
    public Guid? ParentId => CityId;

    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public Guid GovernmentId { get; set; }
    public Government? Government { get; set; }

    public Guid CityId { get; set; }
    public City? City { get; set; }
}