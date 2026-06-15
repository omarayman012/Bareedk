namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Update;

public sealed class UpdateServiceBusinessPlanCommandValidator
: AbstractValidator<UpdateServiceBusinessPlanCommand>
{
    public UpdateServiceBusinessPlanCommandValidator(
    IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(localizer["IdIsRequired"]);

    RuleFor(x => x)
        .Must(x =>
            !string.IsNullOrWhiteSpace(x.NameEn) ||
            !string.IsNullOrWhiteSpace(x.NameAr))
        .WithMessage(localizer["AtLeastOneNameRequired"]);

        RuleFor(x => x.NameEn)
            .MaximumLength(200)
            .WithMessage(localizer["NameEnMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.NameAr)
            .MaximumLength(200)
            .WithMessage(localizer["NameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(1000)
            .WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionEn is not null);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(1000)
            .WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionAr is not null);

        RuleFor(x => x.Image!)
            .Must(file => file.Length > 0)
            .WithMessage(localizer["InvalidImageFile"])
            .Must(file => Path.GetExtension(file.FileName).ToLower() == ".svg")
            .WithMessage(localizer["OnlySvgAllowed"])
            .Must(file => file.Length <= 2 * 1024 * 1024)
            .WithMessage(localizer["ImageSizeExceeded"])
            .When(x => x.Image is not null);
    }

}
