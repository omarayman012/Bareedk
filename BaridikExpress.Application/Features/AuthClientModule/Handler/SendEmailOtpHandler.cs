using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class SendEmailOtpHandler
        : IRequestHandler<SendEmailOtpCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public SendEmailOtpHandler(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(
            SendEmailOtpCommand request,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.NormalizedEmail == normalizedEmail,
                    cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            if (user.EmailConfirmed)
                return Result<string>.Failure("Email already confirmed", 400);

            // ⛔ شرط واحد فقط: دقيقتين بين كل إرسال
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

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<string>.Failure("Failed to save OTP", 500);
            }

            try
            {
                await _emailService.SendConfirmationEmail(user, otp);
            }
            catch
            {
                return Result<string>.Failure("Failed to send OTP email", 500);
            }

            return Result<string>.Success(
                "OTP sent successfully",
                "Success",
                200);
        }
    }
}