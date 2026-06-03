namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;

public class CreateGovernmentCommandValidator
    : AbstractValidator<CreateGovernmentCommand>
{
    public CreateGovernmentCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn))
            .WithMessage(localizer["AtLeastOneGovernmentNameRequired"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z\s]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage(localizer["CountryIdRequired"])
            .Must(BeValidGuid)
            .WithMessage(localizer["CountryIdMustBeValid"]);
    }

    private static bool BeValidGuid(Guid countryId)
    {
        return countryId != Guid.Empty;
    }
}