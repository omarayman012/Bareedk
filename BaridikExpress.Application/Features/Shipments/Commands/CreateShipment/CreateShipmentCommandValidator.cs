using BaridikExpress.Application.Features.Shipments.DTOs;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandValidator
    : AbstractValidator<CreateShipmentCommand>
{
    private const int MaxAttachments = 10;
    private const int MaxServices = 50;
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;
    private const decimal MaxWeight = 9999.99m;
    private const decimal MaxAmount = 999_999.99m;

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".pdf", ".mp4", ".mov", ".avi", ".webm" };

    public CreateShipmentCommandValidator(IStringLocalizer localizer)
    {
        #region Addresses

        RuleFor(x => x.SenderAddress)
            .NotNull().WithMessage(localizer["SenderAddressRequired"])
            .SetValidator(new ShipmentAddressDtoValidator(localizer));

        RuleFor(x => x.ReceiverAddress)
            .NotNull().WithMessage(localizer["ReceiverAddressRequired"])
            .SetValidator(new ShipmentAddressDtoValidator(localizer));

        #endregion

        #region Weight & Pieces

        RuleFor(x => x.TotalWeight)
            .GreaterThan(0).WithMessage(localizer["TotalWeightMustBePositive"])
            .LessThanOrEqualTo(MaxWeight).WithMessage(localizer["TotalWeightExceeded"]);

        RuleFor(x => x.NumberOfPieces)
            .GreaterThan(0).WithMessage(localizer["NumberOfPiecesMustBePositive"])
            .LessThanOrEqualTo(9999).WithMessage(localizer["NumberOfPiecesExceeded"]);

        #endregion

        #region Vehicle & Delivery

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage(localizer["VehicleIdRequired"]);

        RuleFor(x => x.DeliveryTypeId)
            .NotEmpty().WithMessage(localizer["DeliveryTypeIdRequired"]);

        #endregion

        #region Payment

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage(localizer["TotalAmountMustBePositive"])
            .LessThanOrEqualTo(MaxAmount).WithMessage(localizer["TotalAmountExceeded"]);

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage(localizer["InvalidPaymentMethod"]);

        #endregion

        #region Content Type

        RuleFor(x => x.ContentType)
            .IsInEnum().WithMessage(localizer["InvalidContentType"]);

        #endregion

        #region Terms

        RuleFor(x => x.AcceptTermsAndConditions)
            .Equal(true).WithMessage(localizer["MustAcceptTermsAndConditions"]);

        #endregion

        #region Services

        RuleFor(x => x.Services)
            .NotEmpty().WithMessage(localizer["ServicesRequired"])
            .Must(s => s.Count <= MaxServices).WithMessage(localizer["ServicesCountExceeded"]);

        RuleForEach(x => x.Services)
            .SetValidator(new ShipmentServiceDtoValidator(localizer));

        RuleFor(x => x.Services)
            .Must(s => s.Select(x => x.ServiceId).Distinct().Count() == s.Count)
            .WithMessage(localizer["DuplicateServicesNotAllowed"]);

        #endregion

        #region Attachments

        When(x => x.Attachments is { Count: > 0 }, () =>
        {
            RuleFor(x => x.Attachments!)
                .Must(a => a.Count <= MaxAttachments)
                .WithMessage(localizer["AttachmentsCountExceeded"]);

            RuleForEach(x => x.Attachments!)
                .SetValidator(new IFormFileValidator(localizer, AllowedExtensions, MaxFileSizeBytes));
        });

        #endregion

        #region Optional Fields

        When(x => x.Notes is not null, () =>
            RuleFor(x => x.Notes!)
                .MaximumLength(1000).WithMessage(localizer["NotesTooLong"]));

        When(x => x.ExpectedSendingDate.HasValue, () =>
            RuleFor(x => x.ExpectedSendingDate!.Value)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage(localizer["ExpectedDateInPast"]));

        #endregion
    }
}

internal sealed class ShipmentAddressDtoValidator
    : AbstractValidator<ShipmentAddressDto>
{
    public ShipmentAddressDtoValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(localizer["FullNameRequired"])
            .MaximumLength(200).WithMessage(localizer["FullNameTooLong"]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(localizer["PhoneNumberRequired"])
            .Matches(@"^\+?[0-9]{7,15}$").WithMessage(localizer["InvalidPhoneNumber"]);

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage(localizer["CountryIdRequired"]);

        RuleFor(x => x.GovernmentId)
            .NotEmpty().WithMessage(localizer["GovernmentIdRequired"]);

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage(localizer["CityIdRequired"]);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(localizer["AddressRequired"])
            .MaximumLength(500).WithMessage(localizer["AddressTooLong"]);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage(localizer["InvalidLatitude"]);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage(localizer["InvalidLongitude"]);
    }
}

internal sealed class ShipmentServiceDtoValidator
    : AbstractValidator<ShipmentServiceDto>
{
    public ShipmentServiceDtoValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage(localizer["ServiceIdRequired"]);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(localizer["ServiceQuantityMustBePositive"])
            .LessThanOrEqualTo(999).WithMessage(localizer["ServiceQuantityExceeded"]);
    }
}

internal sealed class IFormFileValidator
    : AbstractValidator<IFormFile>
{
    public IFormFileValidator(
        IStringLocalizer localizer,
        HashSet<string> allowedExt,
        long maxSize)
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage(localizer["FileNameRequired"])
            .Must(f => allowedExt.Contains(Path.GetExtension(f)))
            .WithMessage(localizer["InvalidFileExtension"]);

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage(localizer["FileStreamRequired"])
            .LessThanOrEqualTo(maxSize).WithMessage(localizer["FileSizeExceeded"]);
    }
}