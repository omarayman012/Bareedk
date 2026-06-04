namespace BaridikExpress.Application.Features.Vehicles.Commands.CreateVehicles
{
    public class CreateVehicleValidator
        : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleValidator(
            IStringLocalizer localizer)
        {
            RuleFor(x => x)
                  .Must(x =>
                      !string.IsNullOrWhiteSpace(x.NameAr) ||
                      !string.IsNullOrWhiteSpace(x.NameEn)
                  )
                   .WithMessage(localizer["VehicleNameRequired"]);

            RuleFor(x => x.LoadCapacityFrom)
                      .NotNull()
                      .WithMessage(localizer["LoadCapacityFromRequired"])
                      .GreaterThanOrEqualTo(0)
                      .WithMessage(localizer["LoadCapacityFromInvalid"]);

            RuleFor(x => x.LoadCapacityTo)
                .NotNull()
                .WithMessage(localizer["LoadCapacityToRequired"])
                .GreaterThan(0)
                .WithMessage(localizer["LoadCapacityToInvalid"])
                .GreaterThanOrEqualTo(x => x.LoadCapacityFrom)
                .WithMessage(localizer["LoadCapacityRangeInvalid"]);

            RuleFor(x => x.PricePerTon)
                .NotNull()
                .WithMessage(localizer["PricePerTonRequired"])
                .GreaterThan(0)
                .WithMessage(localizer["PricePerTonInvalid"]);

            RuleFor(x => x.Currency)
                .IsInEnum()
                .WithMessage(localizer["InvalidCurrency"]);
            RuleFor(x => x.ImageUrl)
                    .NotNull()
                     .WithMessage(localizer["ImageRequired"])
            .Must(file => file!.Length <= 5 * 1024 * 1024)
            .WithMessage(localizer["ImageMaxSize"])
            .Must(file => new[] { "image/jpeg", "image/png" }.Contains(file!.ContentType))
            .WithMessage(localizer["ImageInvalidType"])
            .When(x => x.ImageUrl != null);
        }
    }
}