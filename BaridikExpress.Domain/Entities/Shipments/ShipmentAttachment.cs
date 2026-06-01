using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Enums;

namespace BaridikExpress.Domain.Entities.Shipments;

public class ShipmentAttachment : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = null!;

    public string FileUrl { get; set; } = string.Empty;
    public AttachmentType Type { get; set; }               // Image, Video, Document
}