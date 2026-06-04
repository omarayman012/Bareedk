namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;

public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .WithMessage(localizer["NameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .WithMessage(localizer["NameEnMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.PhoneCode)
            .MaximumLength(10)
            .WithMessage(localizer["PhoneCodeMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneCode));

        RuleFor(x => x.PhoneCode)
            .Matches(@"^\+?[0-9]+$")
            .WithMessage(localizer["InvalidPhoneCode"])
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneCode));

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn) ||
                !string.IsNullOrWhiteSpace(x.PhoneCode))
            .WithMessage(localizer["AtLeastOneFieldRequired"]);
    }
}