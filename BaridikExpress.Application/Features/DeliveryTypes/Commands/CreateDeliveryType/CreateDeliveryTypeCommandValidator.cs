using FluentValidation;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;

public class CreateDeliveryTypeCommandValidator : AbstractValidator<CreateDeliveryTypeCommand>
{
    public CreateDeliveryTypeCommandValidator(IStringLocalizer localizer)
    {
        #region Name - على الأقل واحد مطلوب

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

        #endregion

        #region Days

        RuleFor(x => x.DaysFrom)
            .GreaterThan(0).WithMessage(localizer["DaysFromMustBeGreaterThanZero"]);

        RuleFor(x => x.DaysTo)
            .GreaterThan(0).WithMessage(localizer["DaysToMustBeGreaterThanZero"])
            .GreaterThanOrEqualTo(x => x.DaysFrom).WithMessage(localizer["DaysToMustBeGreaterThanOrEqualDaysFrom"]);

        #endregion

        #region Price

        RuleFor(x => x.PricePerShipment)
            .GreaterThan(0).WithMessage(localizer["PricePerShipmentMustBeGreaterThanZero"]);

        #endregion

        #region Currency

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage(localizer["InvalidCurrency"]);

        #endregion

        #region Description - على الأقل واحد لو اتبعت

        When(x => x.DescriptionEn is not null || x.DescriptionAr is not null, () =>
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.DescriptionEn) || !string.IsNullOrWhiteSpace(x.DescriptionAr))
                .WithMessage(localizer["AtLeastOneDescriptionRequired"])
                .OverridePropertyName("Description");
        });

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

        #endregion

        #region Image

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