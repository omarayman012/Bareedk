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
    public class ForgotPasswordByPhoneHandler
     : IRequestHandler<ForgotPasswordByPhoneCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ISmsService _smsService;

        public ForgotPasswordByPhoneHandler(
            UserManager<User> userManager,
            ISmsService smsService)
        {
            _userManager = userManager;
            _smsService = smsService;
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
                return Result<string>.Failure("User not found", 404);

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpireAt = DateTime.UtcNow.AddMinutes(10);
            user.ResetPasswordOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _smsService.SendSmsAsync(
                user.PhoneNumber!,
                $"Reset Password OTP: {otp}");

            return Result<string>.Success("OTP sent to phone", "Success", 200);
        }
    }
}
