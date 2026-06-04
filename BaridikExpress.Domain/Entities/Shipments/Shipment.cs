using System.Net.Mime;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.DeliveryType;
using BaridikExpress.Domain.Entities.Enums;
using BaridikExpress.Domain.Entities.Vehicles;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Shipments;

public class Shipment : BaseEntity
{
    public Guid Id { get; private set; }

    #region User

    public string UserId { get; private set; } = string.Empty;
    public User User { get; private set; } = null!;

    #endregion

    #region Addresses

    public Guid SenderAddressId { get; private set; }
    public ShipmentAddress SenderAddress { get; private set; } = null!;

    public Guid ReceiverAddressId { get; private set; }
    public ShipmentAddress ReceiverAddress { get; private set; } = null!;

    #endregion

    #region Shipment Details

    public string? TrackingId { get; private set; }
    public string? Notes { get; private set; }
    public DateOnly? ExpectedSendingDate { get; private set; }
    public decimal TotalWeight { get; private set; }
    public int NumberOfPieces { get; private set; }
    public bool HasDimensions { get; private set; }
    public AttachmentType ContentType { get; private set; }

    #endregion

    #region Vehicle & Delivery

    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;

    public Guid DeliveryTypeId { get; private set; }
    public DeliveryType.DeliveryType DeliveryType { get; private set; } = null!;

    #endregion

    #region Payment

    public decimal TotalAmount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    #endregion

    #region Status

    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Processing;

    #endregion

    #region Collections

    public ICollection<ShipmentAttachment> Attachments { get; private set; }
        = new List<ShipmentAttachment>();

    public ICollection<ShipmentService> ShipmentServices { get; private set; }
        = new List<ShipmentService>();

    public ICollection<ShipmentStatusHistory> StatusHistory { get; private set; }
        = new List<ShipmentStatusHistory>();

    #endregion

    private Shipment() { }

    #region Factory Method

    public static Shipment Create(
        string userId,
        Guid senderAddressId,
        Guid receiverAddressId,
        Guid vehicleId,
        Guid deliveryTypeId,
        decimal totalWeight,
        int numberOfPieces,
        decimal totalAmount,
        PaymentMethod paymentMethod,
        AttachmentType contentType,
        DateOnly? expectedSendingDate = null,
        string? notes = null,
        bool hasDimensions = false)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId is required.", nameof(userId));

        if (senderAddressId == Guid.Empty)
            throw new ArgumentException("SenderAddressId is required.", nameof(senderAddressId));

        if (receiverAddressId == Guid.Empty)
            throw new ArgumentException("ReceiverAddressId is required.", nameof(receiverAddressId));

        if (totalWeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalWeight), "Total weight must be greater than zero.");

        if (numberOfPieces <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfPieces), "Number of pieces must be greater than zero.");

        if (totalAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount cannot be negative.");

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SenderAddressId = senderAddressId,
            ReceiverAddressId = receiverAddressId,
            VehicleId = vehicleId,
            DeliveryTypeId = deliveryTypeId,
            TotalWeight = totalWeight,
            NumberOfPieces = numberOfPieces,
            TotalAmount = totalAmount,
            PaymentMethod = paymentMethod,
            ContentType = contentType,
            ExpectedSendingDate = expectedSendingDate,
            Notes = notes?.Trim(),
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

    #endregion

    #region Behaviour

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
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        TotalAmount = amount;
    }

    public void AddService(ShipmentService service)
    {
        ShipmentServices.Add(service);
    }

    public void AddAttachment(ShipmentAttachment attachment)
    {
        Attachments.Add(attachment);
    }

    #endregion

    #region Helpers

    private static string GenerateTrackingId()
        => $"BRD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

    public bool CanBeCancelled()
        => Status == ShipmentStatus.Processing;

    public bool IsDelivered()
        => Status == ShipmentStatus.Delivered;

    #endregion
}