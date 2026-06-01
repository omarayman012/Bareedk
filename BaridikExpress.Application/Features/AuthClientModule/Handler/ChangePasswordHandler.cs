using BaridikExpress.Application.Features.AuthClientModule.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class ChangePasswordHandler
      : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            // check new password confirm
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result<string>.Failure(
                    "New password and confirm password do not match",
                    400);
            }

            // get user
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("User not found", 404);
            }

            // change password
            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));

                return Result<string>.Failure(errors, 400);
            }

            return Result<string>.Success(
                "Password changed successfully",
                "Success",
                200);
        }
    }
}
