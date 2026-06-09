using FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Banners.Commands.CreateBanner;

public class CreateBannerCommandValidator : AbstractValidator<CreateBannerCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public CreateBannerCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage(localizer["ImageRequired"])
            .Must(file => file!.Length > 0)
            .WithMessage(localizer["InvalidImageFile"])
            .Must(file => AllowedImageExtensions.Contains(
                Path.GetExtension(file!.FileName).ToLower()))
            .WithMessage(localizer["InvalidImageFormat"]);

        RuleFor(x => x.TitleAr)
            .MaximumLength(200)
            .WithMessage(localizer["BannerTitleMaxLength"])
            .When(x => x.TitleAr is not null);

        RuleFor(x => x.TitleEn)
            .MaximumLength(200)
            .WithMessage(localizer["BannerTitleMaxLength"])
            .When(x => x.TitleEn is not null);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(2000)
            .WithMessage(localizer["BannerDescriptionMaxLength"])
            .When(x => x.DescriptionAr is not null);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(2000)
            .WithMessage(localizer["BannerDescriptionMaxLength"])
            .When(x => x.DescriptionEn is not null);
    }
}
