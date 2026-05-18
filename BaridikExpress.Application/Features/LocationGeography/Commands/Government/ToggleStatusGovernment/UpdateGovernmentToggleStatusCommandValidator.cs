using BaridikExpress.Application.Features.LocationGeography.Commands.Government.ToggleStatusGovernment;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateToggleStatus;

public class UpdateGovernmentToggleStatusCommandValidator
    : AbstractValidator<UpdateGovernmentToggleStatusCommand>
{
    public UpdateGovernmentToggleStatusCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["GovernmentIdRequired"]);
    }
}