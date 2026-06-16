namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;

public class CreateCountryValidator : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn))
            .WithMessage(localizer["AtLeastOneCountryNameRequired"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .WithMessage(localizer["CountryNameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .WithMessage(localizer["CountryNameEnMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.PhoneCode)
            .NotEmpty()
            .WithMessage(localizer["PhoneCodeRequired"])
            .MaximumLength(10)
            .WithMessage(localizer["PhoneCodeMaxLength"])
            .Matches(@"^\+?[0-9]+$")
            .WithMessage(localizer["InvalidPhoneCode"]);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .WithMessage(localizer["PostalCodeMaxLength"])
            .Matches(@"^[a-zA-Z0-9\s\-]+$")
            .WithMessage(localizer["InvalidPostalCode"])
            .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));
    }
}