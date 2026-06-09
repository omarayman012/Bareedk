namespace BaridikExpress.Application.Features.Notification.Commands.Delete;

public sealed class DeleteNotificationsCommandValidator
    : AbstractValidator<DeleteNotificationsCommand>
{
    public DeleteNotificationsCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(localizer["IdsAreRequired"]);
    }
}