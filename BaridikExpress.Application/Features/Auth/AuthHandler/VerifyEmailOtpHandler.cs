using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class VerifyEmailOtpHandler
        : IRequestHandler<VerifyEmailOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public VerifyEmailOtpHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
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
                    "User not found",
                    404);
            }

            if (string.IsNullOrWhiteSpace(user.EmailOtp))
            {
                return Result<string>.Failure(
                    "OTP not found",
                    400);
            }

            if (string.IsNullOrWhiteSpace(request.OTP))
            {
                return Result<string>.Failure(
                    "OTP is required",
                    400);
            }

            if (!string.Equals(user.EmailOtp, request.OTP))
            {
                return Result<string>.Failure(
                    "Invalid OTP",
                    400);
            }

            if (!user.EmailOtpExpireAt.HasValue ||
                user.EmailOtpExpireAt.Value < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    "OTP expired",
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
                    "Failed to verify email",
                    500);
            }

            return Result<string>.Success(
                "Email verified successfully",
                "Success",
                200);
        }
    }
}