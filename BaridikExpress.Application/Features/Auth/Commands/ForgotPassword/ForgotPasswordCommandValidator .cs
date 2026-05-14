using BaridikExpress.Application.Consts;

namespace BaridikExpress.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["EmailRequired"])
                .Matches(RegexPatterns.Email)
                .WithMessage(localizer["InvalidEmail"]);
        }
    }
}