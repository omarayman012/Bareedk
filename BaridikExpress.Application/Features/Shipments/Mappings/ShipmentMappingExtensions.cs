using BaridikExpress.Application.Features.Shipments.DTOs;
using BaridikExpress.Domain.Entities.Shipments;

namespace BaridikExpress.Application.Features.Shipments.Mappings;

public static class ShipmentMappingExtensions
{
    public static ShipmentResponse ToResponse(
        this Shipment shipment,
        Dictionary<Guid, dynamic> dbServices) =>
        new(
            Id: shipment.Id,
            TrackingId: shipment.TrackingId!,
            SenderAddress: shipment.SenderAddress.ToSummary(),
            ReceiverAddress: shipment.ReceiverAddress.ToSummary(),
            TotalWeight: shipment.TotalWeight,
            NumberOfPieces: shipment.NumberOfPieces,
            TotalAmount: shipment.TotalAmount,
            PaymentMethod: shipment.PaymentMethod,
            Status: shipment.Status,
            ExpectedSendingDate: shipment.ExpectedSendingDate,
            Notes: shipment.Notes,
            Services: shipment.ShipmentServices
                                     .Select(s => s.ToSummary(dbServices))
                                     .ToList(),
            CreatedAt: shipment.CreatedAt);

    private static ShipmentAddressSummary ToSummary(
        this ShipmentAddress a) =>
        new(a.FullName, a.PhoneNumber,
            a.Country.CountryNameEn, a.Government.GovernmentNameEn,
            a.City.CityNameEn, a.Address);

    private static ShipmentServiceSummary ToSummary(
        this ShipmentService s,
        Dictionary<Guid, dynamic> dbServices) =>
        new(s.ServiceId,
            dbServices[s.ServiceId].NameEn,
            s.Quantity,
            s.UnitPrice,
            s.TotalPrice);
}