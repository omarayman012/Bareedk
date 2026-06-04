using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ConfirmResetPasswordEmailHandler
        : IRequestHandler<ConfirmResetPasswordEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public ConfirmResetPasswordEmailHandler(
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            ConfirmResetPasswordEmailCommand request,
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

            if (user.ResetPasswordOtp != request.Otp)
            {
                return Result<string>.Failure(
                    _localizer["InvalidOtp"],
                    400);
            }

            if (user.ResetPasswordOtpExpireAt < DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpExpired"],
                    400);
            }

            return Result<string>.Success(
                _localizer["OtpConfirmedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}