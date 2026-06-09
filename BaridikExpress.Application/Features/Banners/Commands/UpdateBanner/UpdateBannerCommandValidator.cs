using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Banners.Commands.UpdateBanner;

public class UpdateBannerCommandValidator : AbstractValidator<UpdateBannerCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public UpdateBannerCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["IdRequired"]);

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

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length > 0)
            .WithMessage(localizer["InvalidImageFile"])
            .Must(file => file == null ||
                AllowedImageExtensions.Contains(
                    Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(localizer["InvalidImageFormat"])
            .When(x => x.Image is not null);
    }
}
