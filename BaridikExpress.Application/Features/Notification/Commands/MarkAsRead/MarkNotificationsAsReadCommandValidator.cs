namespace BaridikExpress.Application.Features.Notification.Commands.MarkAsRead;

public sealed class MarkNotificationsAsReadCommandValidator
    : AbstractValidator<MarkNotificationsAsReadCommand>
{
    public MarkNotificationsAsReadCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.NotificationIds)
            .NotEmpty().WithMessage(localizer["IdsAreRequired"]);
    }
}