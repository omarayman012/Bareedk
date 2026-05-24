using BaridikExpress.Application.Features.AuthClientModule.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class VerifyPhoneOtpHandler
        : IRequestHandler<VerifyPhoneOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public VerifyPhoneOtpHandler(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            VerifyPhoneOtpCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.PhoneNumber == request.PhoneNumber,
                    cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure(
                    "User not found",
                    404);
            }

            if (string.IsNullOrWhiteSpace(user.PhoneOtp))
            {
                return Result<string>.Failure(
                    "OTP not found",
                    400);
            }

            if (user.PhoneOtp != request.Otp)
            {
                return Result<string>.Failure(
                    "Invalid OTP",
                    400);
            }

            if (user.PhoneOtpExpireAt < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    "OTP expired",
                    400);
            }

            user.PhoneNumberConfirmed = true;

            user.PhoneOtp = null;
            user.PhoneOtpExpireAt = null;
            user.PhoneOtpLastSentAt = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure(
                    "Failed to verify phone",
                    500);
            }

            return Result<string>.Success(
                "Phone verified successfully",
                "Success",
                200);
        }
    }
}