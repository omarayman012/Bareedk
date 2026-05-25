using FluentValidation;

namespace BaridikExpress.Application.Features.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusValidator : AbstractValidator<ToggleServiceStatusCommand>
{
    public ToggleServiceStatusValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["IdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidId"]);

        #endregion
    }
}