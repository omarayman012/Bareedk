using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class City: BaseEntity
{
    public Guid CityId { get; set; }
    public string CityNameAr { get; set; } = string.Empty;
    public string CityNameEn { get; set; } = string.Empty;
    public Guid GovernmentId { get; set; }
    public Government? Government { get; set; }
    public ICollection<Village>? Villages { get; set; }
}
