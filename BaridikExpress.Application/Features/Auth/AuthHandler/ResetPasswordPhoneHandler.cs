using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ResetPasswordPhoneHandler
        : IRequestHandler<ResetPasswordPhoneCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordPhoneHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ResetPasswordPhoneCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (user == null)
                return Result<string>.Failure("User not found", 404);

            // 🔥 لازم يكون عمل confirm OTP قبل كده
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

            // 🔥 تنظيف كل حاجة
            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpireAt = null;
            user.ResetPasswordOtpLastSentAt = null;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success("Password reset successfully", "Success", 200);
        }
    }
}
