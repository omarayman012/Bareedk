using BaridikExpress.Domain.Entities.Enums;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Shipments.DTOs;

public sealed record ShipmentAddressDto(
    string FullName,
    string PhoneNumber,
    Guid CountryId,
    Guid GovernmentId,
    Guid CityId,
    Guid? VillageId,
    string Address,
    decimal Latitude,
    decimal Longitude,
    string? FloorNumber = null,
    string? ApartmentNumber = null,
    string? Landmark = null,
    string? PostalCode = null);

public sealed record ShipmentServiceDto(
    Guid ServiceId,
    int Quantity);

public sealed record ShipmentCreateResponse(
    Guid Id,
    string TrackingId);

public sealed record ShipmentResponse(
    Guid Id,
    string TrackingId,
    ShipmentAddressSummary SenderAddress,
    ShipmentAddressSummary ReceiverAddress,
    decimal TotalWeight,
    int NumberOfPieces,
    decimal TotalAmount,
    PaymentMethod PaymentMethod,
    ShipmentStatus Status,
    DateOnly? ExpectedSendingDate,
    string? Notes,
    IReadOnlyList<ShipmentServiceSummary> Services,
    IReadOnlyList<string> AttachmentUrls,
    DateTime CreatedAt);

public sealed record ShipmentAddressSummary(
    string FullName,
    string PhoneNumber,
    string Country,
    string Government,
    string City,
    string Address);

public sealed record ShipmentServiceSummary(
    Guid ServiceId,
    string ServiceName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);