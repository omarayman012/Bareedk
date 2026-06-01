using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateAboutUs;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.SystemManagement.Validators;

public class UpdateAboutUsCommandValidator : AbstractValidator<UpdateAboutUsCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png"];
    private static readonly string[] AllowedVideoExtensions = [".mp4", ".mov", ".avi", ".mkv", ".webm"];

    public UpdateAboutUsCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.TitleAr)
            .MaximumLength(300).WithMessage(localizer["TitleMaxLength"])
            .When(x => x.TitleAr is not null);

        RuleFor(x => x.TitleEn)
            .MaximumLength(300).WithMessage(localizer["TitleMaxLength"])
            .When(x => x.TitleEn is not null);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(5000).WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionAr is not null);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(5000).WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.DescriptionEn is not null);

        RuleFor(x => x.ExternalLinkYoutube)
            .MaximumLength(500).WithMessage(localizer["UrlMaxLength"])
            .Must(url => url == null || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            .WithMessage(localizer["InvalidYoutubeUrl"])
            .When(x => x.ExternalLinkYoutube is not null);

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length > 0)
            .WithMessage(localizer["InvalidImageFile"])
            .Must(file => file == null ||
                AllowedImageExtensions.Contains(
                    Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(localizer["InvalidImageFormat"])
            .When(x => x.Image is not null);

        RuleFor(x => x.Video)
            .Must(file => file == null || file.Length > 0)
            .WithMessage(localizer["InvalidVideoFile"])
            .Must(file => file == null ||
                AllowedVideoExtensions.Contains(
                    Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(localizer["InvalidVideoFormat"])
            .When(x => x.Video is not null);
    }
}