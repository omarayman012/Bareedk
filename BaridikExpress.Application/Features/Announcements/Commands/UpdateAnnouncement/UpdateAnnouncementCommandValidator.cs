using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Announcements.Commands.UpdateAnnouncement;

public class UpdateAnnouncementCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
{
    public UpdateAnnouncementCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["IdRequired"]);

        RuleFor(x => x.TextColor)
        .Matches("^#(?:[0-9A-Fa-f]{6})$")
        .WithMessage(localizer["InvalidColorFormat"])
        .When(x => !string.IsNullOrWhiteSpace(x.TextColor));

        RuleFor(x => x.BackgroundColor)
            .Matches("^#(?:[0-9A-Fa-f]{6})$")
            .WithMessage(localizer["InvalidColorFormat"])
            .When(x => !string.IsNullOrWhiteSpace(x.BackgroundColor));


    }
}
