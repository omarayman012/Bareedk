using BaridikExpress.Application.DTOs.LoginModule;
using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class RefreshTokenHandler
        : IRequestHandler<RefreshTokenCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IStringLocalizer _localizer;

        public RefreshTokenHandler(
            UserManager<User> userManager,
            IJwtService jwtService,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _localizer = localizer;
        }

        public async Task<Result<LoginResponseDto>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["UserNotFound"],
                    404);
            }

            if (user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpireAt < DateTime.UtcNow)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["InvalidRefreshToken"],
                    401);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var newToken = await _jwtService.GenerateToken(user, role);
            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireAt = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return Result<LoginResponseDto>.Success(
                new LoginResponseDto
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Role = role,
                    Token = newToken,
                    RefreshToken = newRefreshToken
                },
                _localizer["TokenRefreshedSuccessfully"],
                200);
        }
    }
}