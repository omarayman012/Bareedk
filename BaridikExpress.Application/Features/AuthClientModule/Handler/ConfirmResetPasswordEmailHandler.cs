using BaridikExpress.Application.Features.AuthClientModule.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class ConfirmResetPasswordEmailHandler
    : IRequestHandler<ConfirmResetPasswordEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmResetPasswordEmailHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ConfirmResetPasswordEmailCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == email, cancellationToken);

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
