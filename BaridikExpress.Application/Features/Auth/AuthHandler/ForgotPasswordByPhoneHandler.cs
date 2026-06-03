using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ForgotPasswordByPhoneHandler
        : IRequestHandler<ForgotPasswordByPhoneCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISmsService _smsService;
        private readonly IStringLocalizer _localizer;

        public ForgotPasswordByPhoneHandler(
            UserManager<User> userManager,
            ISmsService smsService,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _smsService = smsService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            ForgotPasswordByPhoneCommand request,
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

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpireAt = DateTime.UtcNow.AddMinutes(10);
            user.ResetPasswordOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _smsService.SendSmsAsync(
                user.PhoneNumber!,
                $"{_localizer["ResetPasswordOtpPrefix"]}: {otp}"
            );

            return Result<string>.Success(
                _localizer["OtpSentToPhone"],
                _localizer["Success"],
                200);
        }
    }
}