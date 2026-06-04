using System.Net.Mime;
using BaridikExpress.Application.Features.Shipments.DTOs;
using BaridikExpress.Domain.Entities.Enums;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommand : IRequest<Result<ShipmentCreateResponse>>
{
    public Guid VehicleId { get; set; }
    public Guid DeliveryTypeId { get; set; }
    public decimal TotalWeight { get; set; }
    public int NumberOfPieces { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public AttachmentType ContentType { get; set; }
    public DateOnly? ExpectedSendingDate { get; set; }
    public string? Notes { get; set; }
    public bool HasDimensions { get; set; }
    public bool AcceptTermsAndConditions { get; set; }
    public ShipmentAddressDto SenderAddress { get; set; } = default!;
    public ShipmentAddressDto ReceiverAddress { get; set; } = default!;
    public List<ShipmentServiceDto> Services { get; set; } = [];
    public List<IFormFile>? Attachments { get; set; }
}