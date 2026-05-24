using FluentValidation;

namespace BaridikExpress.Application.Features.Customer.Commands.ToggleCustomerStatus;

public sealed class ToggleCustomerStatusValidator : AbstractValidator<ToggleCustomerStatusCommand>
{
    public ToggleCustomerStatusValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["CustomerIdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidCustomerId"]);

        #endregion
    }
}