using BaridikExpress.Application.Features.Shipments.DTOs;
using BaridikExpress.Domain.Entities.Enums;

namespace BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;

public sealed record CreateShipmentCommand(
    Guid ClientId,

    ShipmentAddressDto SenderAddress,
    ShipmentAddressDto ReceiverAddress,

    decimal TotalWeight,
    int NumberOfPieces,
    bool HasDimensions,

    Guid VehicleId,
    Guid DeliveryTypeId,

    decimal TotalAmount,
    PaymentMethod PaymentMethod,

    IReadOnlyList<ShipmentServiceDto> Services,

    IReadOnlyList<ShipmentAttachmentDto>? Attachments = null,

    DateOnly? ExpectedSendingDate = null,
    string? Notes = null)
    : IRequest<Result<ShipmentResponse>>;