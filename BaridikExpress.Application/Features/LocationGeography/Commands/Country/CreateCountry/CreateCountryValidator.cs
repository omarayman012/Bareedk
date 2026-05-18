namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;

public class CreateCountryValidator:AbstractValidator<CreateCountryCommand>
{
    public CreateCountryValidator(IStringLocalizer localizer)
    {
        RuleFor(x=>x.NameAr)
            .NotEmpty().WithMessage(localizer["CountryNameArRequired"])
            .MaximumLength(100).WithMessage(localizer["CountryNameArMaxLength"]);

        RuleFor(x=>x.NameEn)
            .NotEmpty().WithMessage(localizer["CountryNameEnRequired"])
            .MaximumLength(100).WithMessage(localizer["CountryNameEnMaxLength"]);
    }
}
