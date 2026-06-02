using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.CreateVillage;

public class CreateVillageCommandValidator
    : AbstractValidator<CreateVillageCommand>
{
    public CreateVillageCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn))
            .WithMessage(localizer["AtLeastOneVillageNameRequired"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .WithMessage(localizer["VillageNameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .WithMessage(localizer["VillageNameEnMaxLength"])
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage(localizer["VillageNameEnInvalid"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage(localizer["CityIdRequired"])
            .Must(BeValidGuid)
            .WithMessage(localizer["CityIdMustBeValid"]);
    }

    private static bool BeValidGuid(Guid cityId)
    {
        return cityId != Guid.Empty;
    }
}