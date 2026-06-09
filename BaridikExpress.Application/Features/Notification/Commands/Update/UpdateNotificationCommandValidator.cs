namespace BaridikExpress.Application.Features.Notification.Commands.Update;

public sealed class UpdateNotificationCommandValidator
    : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["IdIsRequired"]);

        RuleFor(x => x.TitleAr)
            .MaximumLength(200).WithMessage(localizer["TitleMaxLength"])
            .When(x => x.TitleAr is not null);

        RuleFor(x => x.TitleEn)
            .MaximumLength(200).WithMessage(localizer["TitleMaxLength"])
            .When(x => x.TitleEn is not null);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionAr is not null);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionEn is not null);

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