using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ResetPasswordEmailHandler
        : IRequestHandler<ResetPasswordEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public ResetPasswordEmailHandler(UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            ResetPasswordEmailCommand request,
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

            if (user.ResetPasswordOtp == null)
            {
                return Result<string>.Failure(
                    _localizer["OtpNotVerified"],
                    400);
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result<string>.Failure(
                    _localizer["PasswordsDoNotMatch"],
                    400);
            }

            if (user.ResetPasswordOtpExpireAt < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpExpired"],
                    400);
            }

            // 🔥 Change password safely
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

            // 🔥 clear OTP data
            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpireAt = null;
            user.ResetPasswordOtpLastSentAt = null;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success(
                _localizer["PasswordResetSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}