using FluentValidation;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateToggleStatus;

public class UpdateCityToggleStatusCommandValidator : AbstractValidator<UpdateCityToggleStatusCommand>
{
    public UpdateCityToggleStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("IdRequired");
    }
}