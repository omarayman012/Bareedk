namespace BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields
{
    public class CreateCareerFieldValidator
        : AbstractValidator<CreateCareerFieldCommand>
    {
        public CreateCareerFieldValidator(
            IStringLocalizer localizer)
        {
            RuleFor(x => x)
               .Must(x =>
                   !string.IsNullOrWhiteSpace(x.NameAr) ||
                   !string.IsNullOrWhiteSpace(x.NameEn)
               )
               .WithMessage(localizer["CareerFieldNameRequired"]);
        }
    }
}