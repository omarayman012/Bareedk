using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class SendEmailOtpHandler
        : IRequestHandler<SendEmailOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer _localizer;

        public SendEmailOtpHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _emailService = emailService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            SendEmailOtpCommand request,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.NormalizedEmail == normalizedEmail,
                    cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure(
                    _localizer["UserNotFound"],
                    404);
            }

            if (user.EmailConfirmed)
            {
                return Result<string>.Failure(
                    _localizer["EmailAlreadyConfirmed"],
                    400);
            }

            if (user.EmailOtpLastSentAt.HasValue &&
                user.EmailOtpLastSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpResendTooSoon"],
                    400);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.EmailOtp = otp;
            user.EmailOtpExpireAt = DateTime.UtcNow.AddMinutes(5);
            user.EmailOtpLastSentAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure(
                    _localizer["OtpSavedFailed"],
                    500);
            }

            try
            {
                await _emailService.SendConfirmationEmail(user, otp);
            }
            catch
            {
                return Result<string>.Failure(
                    _localizer["OtpEmailSendFailed"],
                    500);
            }

            return Result<string>.Success(
                _localizer["OtpSentSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}