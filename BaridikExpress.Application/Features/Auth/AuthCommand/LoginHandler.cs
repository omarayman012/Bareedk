using BaridikExpress.Application.DTOs.LoginModule;
using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthCommand
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _context;

        public LoginHandler(
            UserManager<User> userManager,
            IJwtService jwtService,
            IApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<Result<LoginResponseDto>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToUpper();
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

            if (user == null)
                return Result<LoginResponseDto>.Failure("Invalid email or password", 400);

            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword)
                return Result<LoginResponseDto>.Failure("Invalid email or password", 400);

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            var permissions = await _context.RolePermissions
                .Where(rp => rp.Role.Name == role)
                .Select(rp => rp.Permission.PermissionName)
                .ToListAsync(cancellationToken);

            var token = await _jwtService.GenerateToken(user, role, permissions);

            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireAt = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            var response = new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.Id,
                FullName = user.FullName,
                Role = role,
                Permissions = permissions
            };

            return Result<LoginResponseDto>.Success(response, "Login successful", 200);
        }
    }
}
