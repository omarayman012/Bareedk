namespace BaridikExpress.Application.Features.Auth.Commands.VerifyResetOtp
{
    public class VerifyResetOtpCommandValidator : AbstractValidator<VerifyResetOtpCommand>
    {
        public VerifyResetOtpCommandValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["Emailrequired"]);

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage(localizer["Otprequired"])
                .Length(6).WithMessage(localizer["Otpinvalidlength"]);
        }
    }
}