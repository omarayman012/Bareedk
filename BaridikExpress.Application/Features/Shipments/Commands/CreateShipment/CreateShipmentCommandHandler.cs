using BaridikExpress.Application.Features.Shipments.DTOs;
using BaridikExpress.Application.Features.Shipments.Mappings;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Shipments;

namespace BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer<CreateShipmentCommandHandler> localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateShipmentCommand, Result<ShipmentResponse>>
{
    public async Task<Result<ShipmentResponse>> Handle(
        CreateShipmentCommand request,
        CancellationToken cancellationToken)
    {
        var fkError = await ValidateForeignKeysAsync(request, cancellationToken);
        if (fkError is not null)
            return fkError;

        var serviceIds = request.Services.Select(s => s.ServiceId).ToHashSet();
        var dbServices = await db.Services
            .Where(s => serviceIds.Contains(s.Id) && s.IsActive)
            .ToDictionaryAsync(
                s => s.Id,
                s => (dynamic)
                cancellationToken
            );

        if (dbServices.Count != serviceIds.Count)
            return Result<ShipmentResponse>.Failure(localizer["OneOrMoreServicesNotFound"]);

        var senderAddress = MapAddress(request.SenderAddress);
        var receiverAddress = MapAddress(request.ReceiverAddress);

        var attachments = await UploadAttachmentsAsync(request.Attachments, cancellationToken);
        if (attachments is null)
            return Result<ShipmentResponse>.Failure(localizer["AttachmentUploadFailed"], 400);

        var shipment = Shipment.Create(
            clientId: request.ClientId,
            senderAddressId: senderAddress.Id,
            receiverAddressId: receiverAddress.Id,
            vehicleId: request.VehicleId,
            deliveryTypeId: request.DeliveryTypeId,
            totalWeight: request.TotalWeight,
            numberOfPieces: request.NumberOfPieces,
            totalAmount: request.TotalAmount,
            paymentMethod: request.PaymentMethod,
            expectedSendingDate: request.ExpectedSendingDate,
            notes: request.Notes,
            hasDimensions: request.HasDimensions);

        foreach (var item in request.Services)
        {
            shipment.ShipmentServices.Add(new ShipmentService
            {
                ShipmentId = shipment.Id,
                ServiceId = item.ServiceId,
                Quantity = item.Quantity,
                UnitPrice = dbServices[item.ServiceId].Price
            });
        }

        foreach (var att in attachments)
            shipment.Attachments.Add(att);

        db.ShipmentAddresses.Add(senderAddress);
        db.ShipmentAddresses.Add(receiverAddress);
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync(cancellationToken);

        return Result<ShipmentResponse>.Success(
            shipment.ToResponse(dbServices),
            localizer["ShipmentCreatedSuccessfully"],
            201);
    }

    private async Task<Result<ShipmentResponse>?> ValidateForeignKeysAsync(
        CreateShipmentCommand request,
        CancellationToken ct)
    {
        if (!await db.Clients.AnyAsync(x => x.Id == request.ClientId, ct))
            return Result<ShipmentResponse>.Failure(localizer["ClientNotFound"], 404);

        if (!await db.Vehicles.AnyAsync(x => x.Id == request.VehicleId, ct))
            return Result<ShipmentResponse>.Failure(localizer["VehicleNotFound"], 404);

        if (!await db.DeliveryTypes.AnyAsync(x => x.Id == request.DeliveryTypeId, ct))
            return Result<ShipmentResponse>.Failure(localizer["DeliveryTypeNotFound"], 404);

        return null;
    }

    private static ShipmentAddress MapAddress(ShipmentAddressDto dto) =>
        new()
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            CountryId = dto.CountryId,
            GovernmentId = dto.GovernmentId,
            CityId = dto.CityId,
            VillageId = dto.VillageId,
            Address = dto.Address,
            FloorNumber = dto.FloorNumber,
            ApartmentNumber = dto.ApartmentNumber,
            Landmark = dto.Landmark,
            PostalCode = dto.PostalCode,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    private async Task<List<ShipmentAttachment>?> UploadAttachmentsAsync(
        IReadOnlyList<ShipmentAttachmentDto>? attachments,
        CancellationToken ct)
    {
        var result = new List<ShipmentAttachment>();
        if (attachments is null or { Count: 0 })
            return result;

        foreach (var att in attachments)
        {
            if (att.FileStream == null) continue;

            var url = await fileStorage.SaveFileAsync(
                att.FileStream,
                att.FileName,
                "shipment-attachments"); 

            if (url is null) return null;

            result.Add(new ShipmentAttachment
            {
                Id = Guid.NewGuid(),
                FileUrl = url,
                Type = att.Type
            });
        }

        return result;
    }
}