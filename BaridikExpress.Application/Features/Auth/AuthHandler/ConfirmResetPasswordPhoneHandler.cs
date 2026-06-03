using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ConfirmResetPasswordPhoneHandler
      : IRequestHandler<ConfirmResetPasswordPhoneCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmResetPasswordPhoneHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ConfirmResetPasswordPhoneCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.PhoneNumber == request.PhoneNumber,
                    cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            if (user.ResetPasswordOtp != request.Otp)
                return Result<string>.Failure("Invalid OTP", 400);

            if (user.ResetPasswordOtpExpireAt < DateTime.UtcNow)
                return Result<string>.Failure("OTP expired", 400);

            return Result<string>.Success("OTP confirmed", "Success", 200);
        }
    }
}
