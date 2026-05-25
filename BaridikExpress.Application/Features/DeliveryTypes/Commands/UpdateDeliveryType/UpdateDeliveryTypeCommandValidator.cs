using FluentValidation;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.UpdateDeliveryType;

public sealed class UpdateDeliveryTypeCommandValidator : AbstractValidator<UpdateDeliveryTypeCommand>
{
    public UpdateDeliveryTypeCommandValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["IdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidId"]);

        #endregion

        #region Name (if sent)

        When(x => x.NameEn is not null || x.NameAr is not null, () =>
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.NameAr))
                .WithMessage(localizer["AtLeastOneNameRequired"])
                .OverridePropertyName("Name");

            When(x => !string.IsNullOrWhiteSpace(x.NameEn), () =>
            {
                RuleFor(x => x.NameEn)
                    .MaximumLength(200).WithMessage(localizer["NameEnMaxLength"]);
            });

            When(x => !string.IsNullOrWhiteSpace(x.NameAr), () =>
            {
                RuleFor(x => x.NameAr)
                    .MaximumLength(200).WithMessage(localizer["NameArMaxLength"]);
            });
        });

        #endregion

        #region Days (if sent)

        When(x => x.DaysFrom.HasValue, () =>
        {
            RuleFor(x => x.DaysFrom)
                .GreaterThan(0).WithMessage(localizer["DaysFromMustBeGreaterThanZero"]);
        });

        When(x => x.DaysTo.HasValue, () =>
        {
            RuleFor(x => x.DaysTo)
                .GreaterThan(0).WithMessage(localizer["DaysToMustBeGreaterThanZero"]);

            When(x => x.DaysFrom.HasValue, () =>
            {
                RuleFor(x => x.DaysTo)
                    .GreaterThanOrEqualTo(x => x.DaysFrom)
                    .WithMessage(localizer["DaysToMustBeGreaterThanOrEqualDaysFrom"]);
            });
        });

        #endregion

        #region Price (if sent)

        When(x => x.PricePerShipment.HasValue, () =>
        {
            RuleFor(x => x.PricePerShipment)
                .GreaterThan(0).WithMessage(localizer["PricePerShipmentMustBeGreaterThanZero"]);
        });

        #endregion

        #region Description (if sent)

        When(x => x.DescriptionEn is not null || x.DescriptionAr is not null, () =>
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.DescriptionEn) || !string.IsNullOrWhiteSpace(x.DescriptionAr))
                .WithMessage(localizer["AtLeastOneDescriptionRequired"])
                .OverridePropertyName("Description");

            When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn), () =>
            {
                RuleFor(x => x.DescriptionEn)
                    .MaximumLength(1000).WithMessage(localizer["DescriptionEnMaxLength"]);
            });

            When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr), () =>
            {
                RuleFor(x => x.DescriptionAr)
                    .MaximumLength(1000).WithMessage(localizer["DescriptionArMaxLength"]);
            });
        });

        #endregion

        #region Image (if sent)

        When(x => x.Image is not null, () =>
        {
            RuleFor(x => x.Image)
                .Must(file => file!.Length > 0)
                .WithMessage(localizer["InvalidImageFile"])
                .Must(file => new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file!.FileName).ToLower()))
                .WithMessage(localizer["InvalidImageFormat"])
                .Must(file => file!.Length <= 5 * 1024 * 1024)
                .WithMessage(localizer["ImageMaxSize"]);
        });

        #endregion
    }
}