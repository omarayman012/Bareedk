using FluentValidation;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateCity;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("IdRequired");

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage("NameArMaxLength")
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage("NameEnMaxLength")
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameAr)
                    || !string.IsNullOrWhiteSpace(x.NameEn)
                    || x.GovernmentId != Guid.Empty)
            .WithMessage("AtLeastOneFieldRequired");
    }
}