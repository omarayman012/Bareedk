using BaridikExpress.Application.Features.Auth.AuthCommand;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public LogoutHandler(
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.RefreshToken == request.RefreshToken,
                    cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure(
                    _localizer["InvalidRefreshToken"],
                    400);
            }

            // revoke token
            user.RefreshToken = null;
            user.RefreshTokenExpireAt = null;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success(
                _localizer["LoggedOutSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}