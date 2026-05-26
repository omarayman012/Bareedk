using FluentValidation;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.ToggleDeliveryTypeStatus;

public sealed class ToggleDeliveryTypeStatusValidator : AbstractValidator<ToggleDeliveryTypeStatusCommand>
{
    public ToggleDeliveryTypeStatusValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["IdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidId"]);

        #endregion
    }
}