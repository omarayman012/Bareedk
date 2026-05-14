using BaridikExpress.Application.Consts;

namespace BaridikExpress.Application.Features.Auth.Commands.ResendConfirmEmail
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
