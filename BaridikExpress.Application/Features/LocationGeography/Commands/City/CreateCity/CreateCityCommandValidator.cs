using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.CreateCity;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn))
            .WithMessage(localizer["AtLeastOneCityNameRequired"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .WithMessage(localizer["CityNameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .WithMessage(localizer["CityNameEnMaxLength"])
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage(localizer["CityNameEnInvalid"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.GovernmentId)
            .NotEmpty()
            .WithMessage(localizer["GovernmentIdRequired"])
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["GovernmentIdMustBeValid"]);

        RuleFor(x => x.CountryId) 
            .NotEmpty()
            .WithMessage(localizer["CountryIdRequired"])
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["CountryIdMustBeValid"]);
    }
}