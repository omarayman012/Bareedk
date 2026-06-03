using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class VerifyEmailOtpHandler
        : IRequestHandler<VerifyEmailOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public VerifyEmailOtpHandler(
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            VerifyEmailOtpCommand request,
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

            if (string.IsNullOrWhiteSpace(user.EmailOtp))
            {
                return Result<string>.Failure(
                    _localizer["OtpNotFound"],
                    400);
            }

            if (string.IsNullOrWhiteSpace(request.OTP))
            {
                return Result<string>.Failure(
                    _localizer["OtpRequired"],
                    400);
            }

            if (!string.Equals(user.EmailOtp, request.OTP))
            {
                return Result<string>.Failure(
                    _localizer["InvalidOtp"],
                    400);
            }

            if (!user.EmailOtpExpireAt.HasValue ||
                user.EmailOtpExpireAt.Value < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpExpired"],
                    400);
            }

            user.EmailConfirmed = true;

            user.EmailOtp = null;
            user.EmailOtpExpireAt = null;
            user.EmailOtpLastSentAt = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure(
                    _localizer["EmailVerificationFailed"],
                    500);
            }

            return Result<string>.Success(
                _localizer["EmailVerifiedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}