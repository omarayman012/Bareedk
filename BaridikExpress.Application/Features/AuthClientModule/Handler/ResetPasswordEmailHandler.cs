using BaridikExpress.Application.Features.AuthClientModule.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class ResetPasswordEmailHandler
        : IRequestHandler<ResetPasswordEmailCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordEmailHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ResetPasswordEmailCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == email, cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            // 🔥 لازم يكون OTP موجود (confirmed step)
            if (user.ResetPasswordOtp == null)
                return Result<string>.Failure("OTP not verified", 400);

            // check password match
            if (request.NewPassword != request.ConfirmPassword)
                return Result<string>.Failure("Passwords do not match", 400);

            // check expiry
            if (user.ResetPasswordOtpExpireAt < DateTime.UtcNow)
                return Result<string>.Failure("OTP expired", 400);

            // 🔥 تغيير الباسورد
            var hasher = new PasswordHasher<User>();

            user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

            // 🔥 تنظيف البيانات بعد الاستخدام
            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpireAt = null;
            user.ResetPasswordOtpLastSentAt = null;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success(
                "Password reset successfully",
                "Success",
                200);
        }
    }
}
