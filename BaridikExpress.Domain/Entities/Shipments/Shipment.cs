using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.ClientModule;
using BaridikExpress.Domain.Entities.DeliveryType;
using BaridikExpress.Domain.Entities.Enums;
using BaridikExpress.Domain.Entities.Vehicles;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Shipments;

public class Shipment : BaseEntity
{
    public Guid Id { get; private set; }

    // ── Client ──────────────────────────────────────────────────────────────
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;

    // ── Addresses ───────────────────────────────────────────────────────────
    public Guid SenderAddressId { get; private set; }
    public ShipmentAddress SenderAddress { get; private set; } = null!;

    public Guid ReceiverAddressId { get; private set; }
    public ShipmentAddress ReceiverAddress { get; private set; } = null!;

    // ── Shipment details ────────────────────────────────────────────────────
    public string? TrackingId { get; private set; }
    public string? Notes { get; private set; }
    public DateOnly? ExpectedSendingDate { get; private set; }

    public decimal TotalWeight { get; private set; }
    public int NumberOfPieces { get; private set; }
    public bool HasDimensions { get; private set; }

    // ── Vehicle & Delivery ──────────────────────────────────────────────────
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;

    public Guid DeliveryTypeId { get; private set; }
    public DeliveryType.DeliveryType DeliveryType { get; private set; } = null!;

    // ── Payment ─────────────────────────────────────────────────────────────
    public decimal TotalAmount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    // ── Status ──────────────────────────────────────────────────────────────
    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Processing;

    // ── Collections ─────────────────────────────────────────────────────────
    public ICollection<ShipmentAttachment> Attachments { get; private set; }
        = new List<ShipmentAttachment>();

    public ICollection<ShipmentService> ShipmentServices { get; private set; }
        = new List<ShipmentService>();

    public ICollection<ShipmentStatusHistory> StatusHistory { get; private set; }
        = new List<ShipmentStatusHistory>();

    // ── Factory method ──────────────────────────────────────────────────────
    public static Shipment Create(
        Guid clientId,
        Guid senderAddressId,
        Guid receiverAddressId,
        Guid vehicleId,
        Guid deliveryTypeId,
        decimal totalWeight,
        int numberOfPieces,
        decimal totalAmount,
        PaymentMethod paymentMethod,
        DateOnly? expectedSendingDate = null,
        string? notes = null,
        bool hasDimensions = false)
    {
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            SenderAddressId = senderAddressId,
            ReceiverAddressId = receiverAddressId,
            VehicleId = vehicleId,
            DeliveryTypeId = deliveryTypeId,
            TotalWeight = totalWeight,
            NumberOfPieces = numberOfPieces,
            TotalAmount = totalAmount,
            PaymentMethod = paymentMethod,
            ExpectedSendingDate = expectedSendingDate,
            Notes = notes,
            HasDimensions = hasDimensions,
            Status = ShipmentStatus.Processing,
            TrackingId = GenerateTrackingId()
        };

        shipment.StatusHistory.Add(new ShipmentStatusHistory
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Status = ShipmentStatus.Processing,
            ChangedAt = DateTime.UtcNow,
            Note = "Shipment created"
        });

        return shipment;
    }

    // ── Behaviour ────────────────────────────────────────────────────────────
    public void UpdateStatus(ShipmentStatus newStatus, string? note = null)
    {
        Status = newStatus;
        StatusHistory.Add(new ShipmentStatusHistory
        {
            Id = Guid.NewGuid(),
            ShipmentId = Id,
            Status = newStatus,
            ChangedAt = DateTime.UtcNow,
            Note = note
        });
    }

    public void UpdateTotalAmount(decimal amount)
    {
        TotalAmount = amount;
    }

    private static string GenerateTrackingId()
        => $"BRD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
}
