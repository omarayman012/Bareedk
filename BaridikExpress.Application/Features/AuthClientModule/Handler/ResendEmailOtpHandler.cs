using BaridikExpress.Application.Features.AuthClientModule.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class ResendEmailOtpHandler
     : IRequestHandler<ResendEmailOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ResendEmailOtpHandler(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(
            ResendEmailOtpCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == email, cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            if (user.EmailConfirmed)
                return Result<string>.Failure("Email already confirmed", 400);

            if (user.EmailOtpLastSentAt.HasValue &&
                user.EmailOtpLastSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
            {
                return Result<string>.Failure(
                    "Please wait 2 minutes before requesting another OTP",
                    400);
            }

            var otp = Random.Shared.Next(100000, 999999).ToString();

            user.EmailOtp = otp;
            user.EmailOtpExpireAt = DateTime.UtcNow.AddMinutes(5);
            user.EmailOtpLastSentAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _emailService.SendConfirmationEmail(user, otp);

            return Result<string>.Success("OTP resent successfully", "Success", 200);
        }
    }

}
