using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Shipments;

public class ShipmentStatusHistory : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = null!;

    public ShipmentStatus Status { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}
