using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ForgotPasswordByEmailHandler
        : IRequestHandler<ForgotPasswordByEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer _localizer;

        public ForgotPasswordByEmailHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _emailService = emailService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            ForgotPasswordByEmailCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.NormalizedEmail == email,
                    cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure(
                    _localizer["UserNotFound"],
                    404);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpireAt = DateTime.UtcNow.AddMinutes(10);
            user.ResetPasswordOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _emailService.SendResetPasswordEmail(user, otp);

            return Result<string>.Success(
                _localizer["OtpSentToEmail"],
                _localizer["Success"],
                200);
        }
    }
}