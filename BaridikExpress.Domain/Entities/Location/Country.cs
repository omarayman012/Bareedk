using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Location;

public class Country: BaseEntity
{
    public Guid CountryId { get; set; }
    public string CountryNameAr { get; set; } = string.Empty;
    public string CountryNameEn { get; set; } = string.Empty;

    public ICollection<Government>? Governments { get; set; }
}
