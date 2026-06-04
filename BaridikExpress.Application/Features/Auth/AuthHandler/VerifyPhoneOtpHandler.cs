using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class VerifyPhoneOtpHandler
        : IRequestHandler<VerifyPhoneOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public VerifyPhoneOtpHandler(
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
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
                    _localizer["UserNotFound"],
                    404);
            }

            if (string.IsNullOrWhiteSpace(user.PhoneOtp))
            {
                return Result<string>.Failure(
                    _localizer["OtpNotFound"],
                    400);
            }

            if (!string.Equals(user.PhoneOtp, request.Otp))
            {
                return Result<string>.Failure(
                    _localizer["InvalidOtp"],
                    400);
            }

            if (!user.PhoneOtpExpireAt.HasValue ||
                user.PhoneOtpExpireAt.Value < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpExpired"],
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
                    _localizer["PhoneVerificationFailed"],
                    500);
            }

            return Result<string>.Success(
                _localizer["PhoneVerifiedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}