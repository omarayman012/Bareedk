namespace BaridikExpress.Application.Validators.AuthModules
{
    public class ResendConfirmationEmailCommandValidator:AbstractValidator<ResendConfirmationEmailCommand>
    {
        public ResendConfirmationEmailCommandValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.Email)
                 .NotEmpty()
                 .WithMessage(localizer["EmailRequired"])
                 .Matches(RegexPatterns.Email)
                 .WithMessage(localizer["InvalidEmail"]);
        }
    }
}
