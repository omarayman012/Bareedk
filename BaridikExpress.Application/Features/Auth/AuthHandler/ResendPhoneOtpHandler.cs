using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ResendPhoneOtpHandler
        : IRequestHandler<ResendPhoneOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISmsService _smsService;

        public ResendPhoneOtpHandler(
            UserManager<User> userManager,
            ISmsService smsService)
        {
            _userManager = userManager;
            _smsService = smsService;
        }

        public async Task<Result<string>> Handle(
            ResendPhoneOtpCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.PhoneNumber == request.PhoneNumber,
                    cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            if (user.PhoneNumberConfirmed)
                return Result<string>.Failure("Phone already confirmed", 400);

            if (user.PhoneOtpLastSentAt.HasValue &&
                user.PhoneOtpLastSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    "Please wait 2 minutes before requesting another OTP",
                    400);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.PhoneOtp = otp;
            user.PhoneOtpExpireAt = DateTime.UtcNow.AddMinutes(5);
            user.PhoneOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _smsService.SendSmsAsync(
                user.PhoneNumber!,
                $"Your OTP code is: {otp}");

            return Result<string>.Success("OTP resent successfully", "Success", 200);
        }
    }
}
