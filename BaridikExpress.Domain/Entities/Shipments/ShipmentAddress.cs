using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;

namespace BaridikExpress.Domain.Entities.Shipments;

public class ShipmentAddress : BaseEntity
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public Guid CountryId { get; set; }
    public Country Country { get; set; } = null!;

    public Guid GovernmentId { get; set; }
    public Government Government { get; set; } = null!;

    public Guid CityId { get; set; }
    public City City { get; set; } = null!;

    public Guid? VillageId { get; set; }
    public Village? Village { get; set; }

    public string Address { get; set; } = string.Empty;
    public string? FloorNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? Landmark { get; set; }
    public string? PostalCode { get; set; }

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
