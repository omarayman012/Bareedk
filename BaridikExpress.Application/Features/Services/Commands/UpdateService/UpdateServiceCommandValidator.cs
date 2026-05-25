using FluentValidation;

namespace BaridikExpress.Application.Features.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator(IStringLocalizer localizer)
    {
        #region Name (if sent)

        When(x => x.NameEn is not null || x.NameAr is not null, () =>
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.NameAr))
                .WithMessage(localizer["AtLeastOneNameRequired"])
                .OverridePropertyName("Name");

            When(x => !string.IsNullOrWhiteSpace(x.NameEn), () =>
                RuleFor(x => x.NameEn).MaximumLength(200).WithMessage(localizer["NameEnMaxLength"]));

            When(x => !string.IsNullOrWhiteSpace(x.NameAr), () =>
                RuleFor(x => x.NameAr).MaximumLength(200).WithMessage(localizer["NameArMaxLength"]));
        });

        #endregion

        #region Price (if sent)

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(localizer["PriceMustBeGreaterThanZero"]);
        });

        #endregion

        #region Image (if sent)

        When(x => x.Image is not null, () =>
        {
            RuleFor(x => x.Image)
                .Must(f => f!.Length > 0).WithMessage(localizer["InvalidImageFile"])
                .Must(f => new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(f!.FileName).ToLower()))
                .WithMessage(localizer["InvalidImageFormat"])
                .Must(f => f!.Length <= 5 * 1024 * 1024).WithMessage(localizer["ImageMaxSize"]);
        });

        #endregion
    }
}