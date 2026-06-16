using FluentValidation;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Announcements.Commands.CreateAnnouncement;

public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
{
   

    public CreateAnnouncementCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
                 .Must(x =>
                     !string.IsNullOrWhiteSpace(x.TitleEn) ||
                     !string.IsNullOrWhiteSpace(x.TitleAr)
                 )
                  .WithMessage(localizer["AnnouncementTitleRequired"]);

        RuleFor(x => x.TextColor)
    .NotEmpty()
    .WithMessage(localizer["AnnouncementTextColorRequired"])
    .Matches("^#(?:[0-9A-Fa-f]{6})$")
    .WithMessage(localizer["InvalidColorFormat"]);

        RuleFor(x => x.BackgroundColor)
            .NotEmpty()
            .WithMessage(localizer["AnnouncementBackgroundColorRequired"])
            .Matches("^#(?:[0-9A-Fa-f]{6})$")
            .WithMessage(localizer["InvalidColorFormat"]);

    }
}
