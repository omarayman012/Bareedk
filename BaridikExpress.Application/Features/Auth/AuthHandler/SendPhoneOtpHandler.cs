using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class SendPhoneOtpHandler
        : IRequestHandler<SendPhoneOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISmsService _smsService;
        private readonly IStringLocalizer _localizer;

        public SendPhoneOtpHandler(
            UserManager<User> userManager,
            ISmsService smsService,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _smsService = smsService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            SendPhoneOtpCommand request,
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

            if (user.PhoneNumberConfirmed)
            {
                return Result<string>.Failure(
                    _localizer["PhoneAlreadyConfirmed"],
                    400);
            }

            if (user.PhoneOtpLastSentAt.HasValue &&
                user.PhoneOtpLastSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    _localizer["OtpResendTooSoon"],
                    400);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.PhoneOtp = otp;
            user.PhoneOtpExpireAt = DateTime.UtcNow.AddMinutes(5);
            user.PhoneOtpLastSentAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure(
                    _localizer["OtpSavedFailed"],
                    500);
            }

            try
            {
                await _smsService.SendSmsAsync(
                    user.PhoneNumber!,
                    $"{_localizer["PhoneOtpPrefix"]}: {otp}");
            }
            catch
            {
                return Result<string>.Failure(
                    _localizer["OtpSmsSendFailed"],
                    500);
            }

            return Result<string>.Success(
                _localizer["OtpSentSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}