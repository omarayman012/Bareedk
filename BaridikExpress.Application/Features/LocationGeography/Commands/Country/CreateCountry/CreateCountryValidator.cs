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
    }
}