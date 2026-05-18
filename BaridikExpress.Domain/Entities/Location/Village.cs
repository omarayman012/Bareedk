using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class Village:BaseEntity
{
    public Guid VillageId { get; set; }
    public string VillageNameAr { get; set; } = string.Empty;
    public string VillageNameEn { get; set; } = string.Empty;
    public Guid CityId { get; set; }
    public City? City { get; set; }
}
