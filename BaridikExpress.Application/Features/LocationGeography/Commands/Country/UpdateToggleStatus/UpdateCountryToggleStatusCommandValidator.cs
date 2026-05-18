using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateToggleStatus;

public class UpdateCountryToggleStatusCommandValidator
    : AbstractValidator<UpdateCountryToggleStatusCommand>
{
    public UpdateCountryToggleStatusCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["CountryIdRequired"]);
    }
}