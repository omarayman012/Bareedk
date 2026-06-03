using BaridikExpress.Application.Features.Auth.AuthCommand;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public LogoutHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("Invalid refresh token", 400);
            }

            // revoke token
            user.RefreshToken = null;
            user.RefreshTokenExpireAt = null;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success("Logged out successfully", "Success", 200);
        }
    }
}
