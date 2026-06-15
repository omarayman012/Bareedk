namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateGeneralCompanySettings;

public sealed class UpdateGeneralCompanySettingsCommandValidator
    : AbstractValidator<UpdateGeneralCompanySettingsCommand>
{
    public UpdateGeneralCompanySettingsCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.WorkingHoursDuration)
            .GreaterThan(0).WithMessage(localizer["WorkingHoursMustBeGreaterThanZero"])
            .When(x => x.WorkingHoursDuration.HasValue);

        RuleFor(x => x.NumberOfRejectedShipmentsByDelivery)
            .GreaterThanOrEqualTo(0).WithMessage(localizer["NumberOfRejectedShipmentsCannotBeNegative"])
            .When(x => x.NumberOfRejectedShipmentsByDelivery.HasValue);
    }
}