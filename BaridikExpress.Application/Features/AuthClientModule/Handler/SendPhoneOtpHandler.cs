using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class SendPhoneOtpHandler
        : IRequestHandler<SendPhoneOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISmsService _smsService;

        public SendPhoneOtpHandler(
            UserManager<User> userManager,
            ISmsService smsService)
        {
            _userManager = userManager;
            _smsService = smsService;
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
                    "User not found",
                    404);
            }

            if (user.PhoneNumberConfirmed)
            {
                return Result<string>.Failure(
                    "Phone already confirmed",
                    400);
            }

            // لازم يستنى دقيقتين بين كل OTP
            if (user.PhoneOtpLastSentAt.HasValue &&
                user.PhoneOtpLastSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    "Please wait 2 minutes before requesting another OTP",
                    400);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.PhoneOtp = otp;

            // الكود صالح 5 دقايق
            user.PhoneOtpExpireAt = DateTime.UtcNow.AddMinutes(5);

            user.PhoneOtpLastSentAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure(
                    "Failed to save OTP",
                    500);
            }

            try
            {
                await _smsService.SendSmsAsync(
                    user.PhoneNumber!,
                    $"Your OTP code is: {otp}");
            }
            catch
            {
                return Result<string>.Failure(
                    "Failed to send OTP SMS",
                    500);
            }

            return Result<string>.Success(
                "OTP sent successfully",
                "Success",
                200);
        }
    }
}