using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ForgotPasswordByEmailHandler
     : IRequestHandler<ForgotPasswordByEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordByEmailHandler(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(
            ForgotPasswordByEmailCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == email, cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpireAt = DateTime.UtcNow.AddMinutes(10);
            user.ResetPasswordOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _emailService.SendResetPasswordEmail(user, otp);

            return Result<string>.Success("OTP sent to email", "Success", 200);
        }
    }
}
