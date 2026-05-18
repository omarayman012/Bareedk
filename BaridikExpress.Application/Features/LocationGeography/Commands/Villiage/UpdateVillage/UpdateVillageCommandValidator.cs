namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateVillage;

public class UpdateVillageCommandValidator
    : AbstractValidator<UpdateVillageCommand>
{
    public UpdateVillageCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["VillageIdRequired"]);

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.NameAr) ||
                !string.IsNullOrWhiteSpace(x.NameEn) ||
                x.CityId.HasValue)
            .WithMessage(localizer["AtLeastOneFieldRequired"]);

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
            .When(x => x.CityId.HasValue);
    }
}