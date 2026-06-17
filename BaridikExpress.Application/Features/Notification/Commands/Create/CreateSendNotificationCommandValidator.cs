using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Notification.Commands.Create;

public sealed class CreateSendNotificationCommandValidator
    : AbstractValidator<CreateSendNotificationCommand>
{
    public CreateSendNotificationCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.TitleAr) ||
                !string.IsNullOrWhiteSpace(x.TitleEn))
            .WithMessage(localizer["AtLeastOneTitleRequired"]);

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.DescriptionAr) ||
                !string.IsNullOrWhiteSpace(x.DescriptionEn))
            .WithMessage(localizer["AtLeastOneDescriptionRequired"]);

        RuleFor(x => x.TitleAr)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TitleAr))
            .WithMessage(localizer["TitleMaxLength"]);

        RuleFor(x => x.TitleEn)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TitleEn))
            .WithMessage(localizer["TitleMaxLength"]);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr))
            .WithMessage(localizer["DescriptionMaxLength"]);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn))
            .WithMessage(localizer["DescriptionMaxLength"]);

        RuleFor(x => x)
            .Must(x =>
                x.ClientsCreatedByAdmin.Count > 0 ||
                x.DeliveriesCreatedByAdmin.Count > 0 ||
                x.ClientsExternalRegistration.Count > 0 ||
                x.DeliveriesExternalRegistration.Count > 0)
            .WithMessage(localizer["AtLeastOneRecipientRequired"]);

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length > 0)
            .WithMessage(localizer["InvalidImageFile"])
            .Must(file =>
                file == null ||
                new[] { ".jpg", ".jpeg", ".png", ".webp" }
                    .Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage(localizer["InvalidImageFormat"])
            .When(x => x.Image is not null);
    }
}