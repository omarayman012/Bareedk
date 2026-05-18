namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;

public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator(IStringLocalizer _localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(_localizer["IdRequired"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage(_localizer["NameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage(_localizer["NameEnMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn)); 

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameAr)
                    || !string.IsNullOrWhiteSpace(x.NameEn))
            .WithMessage(_localizer["AtLeastOneNameRequired"]);
    }
}