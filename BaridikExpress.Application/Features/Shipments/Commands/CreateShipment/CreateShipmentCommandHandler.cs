using BaridikExpress.Application.Features.Shipments.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Shipments;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<CreateShipmentCommand, Result<ShipmentCreateResponse>>
{
    public async Task<Result<ShipmentCreateResponse>> Handle(
        CreateShipmentCommand request,
        CancellationToken cancellationToken)
    {
        #region Get Current User

        var userId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Result<ShipmentCreateResponse>.Failure(localizer["Unauthorized"], 401);

        #endregion

        #region Validate Foreign Keys

        if (!await db.ApplicationUsers.AnyAsync(x => x.Id == userId, cancellationToken))
            return Result<ShipmentCreateResponse>.Failure(localizer["UserNotFound"], 404);

        if (!await db.Vehicles.AnyAsync(x => x.Id == request.VehicleId, cancellationToken))
            return Result<ShipmentCreateResponse>.Failure(localizer["VehicleNotFound"], 404);

        if (!await db.DeliveryTypes.AnyAsync(x => x.Id == request.DeliveryTypeId, cancellationToken))
            return Result<ShipmentCreateResponse>.Failure(localizer["DeliveryTypeNotFound"], 404);

        #endregion

        #region Validate Address Location FKs

        var addressError = await ValidateAddressAsync(request.SenderAddress, cancellationToken);
        if (addressError is not null)
            return Result<ShipmentCreateResponse>.Failure(addressError);

        addressError = await ValidateAddressAsync(request.ReceiverAddress, cancellationToken);
        if (addressError is not null)
            return Result<ShipmentCreateResponse>.Failure(addressError);

        #endregion

        #region Validate & Fetch Services

        var serviceIds = request.Services.Select(s => s.ServiceId).ToHashSet();

        var dbServices = await db.Services
            .Where(s => serviceIds.Contains(s.Id) && s.IsActive)
            .ToDictionaryAsync(s => s.Id, s => s.Price, cancellationToken);

        if (dbServices.Count != serviceIds.Count)
            return Result<ShipmentCreateResponse>.Failure(localizer["OneOrMoreServicesNotFound"]);

        #endregion

        #region Create Addresses

        var senderAddress = MapAddress(request.SenderAddress);
        var receiverAddress = MapAddress(request.ReceiverAddress);

        db.ShipmentAddresses.Add(senderAddress);
        db.ShipmentAddresses.Add(receiverAddress);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        #region Upload Attachments

        var attachments = new List<ShipmentAttachment>();

        if (request.Attachments is { Count: > 0 })
        {
            foreach (var file in request.Attachments)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();

                var type = ext is ".mp4" or ".mov" or ".avi" or ".webm"
                    ? AttachmentType.Video
                    : AttachmentType.Image;

                var url = await fileStorage.SaveFileAsync(
                    file.OpenReadStream(),
                    file.FileName,
                    "shipment-attachments");

                if (url is null)
                    return Result<ShipmentCreateResponse>.Failure(localizer["AttachmentUploadFailed"], 400);

                attachments.Add(new ShipmentAttachment
                {
                    Id = Guid.NewGuid(),
                    FileUrl = url,
                    Type = type
                });
            }
        }

        #endregion

        #region Create Shipment

        var shipment = Shipment.Create(
            userId: userId,
            senderAddressId: senderAddress.Id,
            receiverAddressId: receiverAddress.Id,
            vehicleId: request.VehicleId,
            deliveryTypeId: request.DeliveryTypeId,
            totalWeight: request.TotalWeight,
            numberOfPieces: request.NumberOfPieces,
            totalAmount: request.TotalAmount,
            paymentMethod: request.PaymentMethod,
            contentType: request.ContentType,
            expectedSendingDate: request.ExpectedSendingDate,
            notes: request.Notes,
            hasDimensions: request.HasDimensions);

        #endregion

        #region Add Services

        foreach (var item in request.Services)
        {
            shipment.AddService(new ShipmentService
            {
                ShipmentId = shipment.Id,
                ServiceId = item.ServiceId,
                Quantity = item.Quantity,
                UnitPrice = dbServices[item.ServiceId]
            });
        }

        #endregion

        #region Add Attachments

        foreach (var att in attachments)
            shipment.AddAttachment(att);

        #endregion

        #region Save

        db.Shipments.Add(shipment);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<ShipmentCreateResponse>.Success(
            new ShipmentCreateResponse(shipment.Id, shipment.TrackingId!),
            localizer["ShipmentCreatedSuccessfully"],
            201);
    }

    #region Helpers

    private async Task<string?> ValidateAddressAsync(
        ShipmentAddressDto dto,
        CancellationToken ct)
    {
        if (!await db.Countries.AnyAsync(x => x.CountryId == dto.CountryId, ct))
            return localizer["CountryNotFound"];

        if (!await db.Governments.AnyAsync(x => x.GovernmentId == dto.GovernmentId, ct))
            return localizer["GovernmentNotFound"];

        if (!await db.Cities.AnyAsync(x => x.CityId == dto.CityId, ct))
            return localizer["CityNotFound"];

        if (dto.VillageId.HasValue &&
            !await db.Villages.AnyAsync(x => x.VillageId == dto.VillageId.Value, ct))
            return localizer["VillageNotFound"];

        return null;
    }

    private static ShipmentAddress MapAddress(ShipmentAddressDto dto) =>
        new()
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName.Trim(),
            PhoneNumber = dto.PhoneNumber.Trim(),
            CountryId = dto.CountryId,
            GovernmentId = dto.GovernmentId,
            CityId = dto.CityId,
            VillageId = dto.VillageId,
            Address = dto.Address.Trim(),
            FloorNumber = dto.FloorNumber?.Trim(),
            ApartmentNumber = dto.ApartmentNumber?.Trim(),
            Landmark = dto.Landmark?.Trim(),
            PostalCode = dto.PostalCode?.Trim(),
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };

    #endregion
}