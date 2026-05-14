using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler(
        UserManager<User> userManager,
        ITokenService tokenService,
        IStringLocalizer localizer
    ) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<LoginResponseDto>.Failure(_localizer["UserNotfound"], 404);

            if (!user.EmailConfirmed)
                return Result<LoginResponseDto>.Failure(_localizer["Accountnotverified"], 400);

            if (user.LockoutEnabled && user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
                return Result<LoginResponseDto>.Failure(_localizer["Accountlocked"], 400);

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validPassword)
                return Result<LoginResponseDto>.Failure(_localizer["Invalidcredentials"], 400);

            var jwt = await _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user);

            var response = new LoginResponseDto
            {
                AccessToken = jwt.Token,
                AccessTokenExpiresAt = jwt.ExpiresAt,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresOn,
                User = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    IsVerified = user.EmailConfirmed
                }
            };

            return Result<LoginResponseDto>.Success(response, _localizer["Loginsuccessful"]);
        }
    }
}