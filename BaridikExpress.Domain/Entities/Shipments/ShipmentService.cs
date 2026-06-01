using BaridikExpress.Domain.Entities.Services;

namespace BaridikExpress.Domain.Entities.Shipments;

public class ShipmentService
{
    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Computed total for this line item (stored for history / price change safety)
    public decimal TotalPrice => Quantity * UnitPrice;
}
