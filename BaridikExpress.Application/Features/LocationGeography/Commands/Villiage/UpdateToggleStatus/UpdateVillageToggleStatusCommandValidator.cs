namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateToggleStatus;

public class UpdateVillageToggleStatusCommandValidator
    : AbstractValidator<UpdateVillageToggleStatusCommand>
{
    public UpdateVillageToggleStatusCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["VillageIdRequired"]);
    }
}