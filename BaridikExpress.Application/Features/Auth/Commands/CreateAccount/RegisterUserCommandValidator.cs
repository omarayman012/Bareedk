using BaridikExpress.Application.Consts;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateAccount
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(IStringLocalizer<RegisterUserCommandValidator> localizer)
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(localizer["FullNameRequired"]);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["EmailRequired"])
                .Matches(RegexPatterns.Email)
                .WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage(localizer["InvalidPhone"]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordRequired"])
                .Matches(RegexPatterns.Password)
                .WithMessage(localizer["WeakPassword"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(localizer["ConfirmPasswordRequired"])
                .Equal(x => x.Password)
                .WithMessage(localizer["PasswordsNotMatch"]);
        }
    }
}