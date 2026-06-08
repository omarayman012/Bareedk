namespace BaridikExpress.Application.Features.Notification.Commands.Create;

public sealed class CreateSendNotificationCommandValidator
    : AbstractValidator<CreateSendNotificationCommand>
{
    public CreateSendNotificationCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.TitleAr)
            .NotEmpty().WithMessage(localizer["TitleArIsRequired"])
            .MaximumLength(200).WithMessage(localizer["TitleMaxLength"]);

        RuleFor(x => x.TitleEn)
            .NotEmpty().WithMessage(localizer["TitleEnIsRequired"])
            .MaximumLength(200).WithMessage(localizer["TitleMaxLength"]);

        RuleFor(x => x.DescriptionAr)
            .NotEmpty().WithMessage(localizer["DescriptionArIsRequired"])
            .MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"]);

        RuleFor(x => x.DescriptionEn)
            .NotEmpty().WithMessage(localizer["DescriptionEnIsRequired"])
            .MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"]);

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
            .Must(file => file == null ||
                new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(localizer["InvalidImageFormat"])
            .When(x => x.Image is not null);
    }
}