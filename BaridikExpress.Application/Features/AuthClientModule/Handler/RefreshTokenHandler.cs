using BaridikExpress.Application.DTOs.LoginModule;
using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class RefreshTokenHandler
     : IRequestHandler<RefreshTokenCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public RefreshTokenHandler(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<Result<LoginResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<LoginResponseDto>.Failure("User not found", 404);

            if (user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpireAt < DateTime.UtcNow)
            {
                return Result<LoginResponseDto>.Failure("Invalid refresh token", 401);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var newToken = await _jwtService.GenerateToken(user, role);
            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireAt = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return Result<LoginResponseDto>.Success(new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Role = role,
                Token = newToken,
                RefreshToken = newRefreshToken
            }, "Token refreshed", 200);
        }


    }
}
