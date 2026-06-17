using BaridikExpress.Application.DTOs.LoginModule;
using BaridikExpress.Application.Features.Auth.Command;
using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthCommand
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public LoginHandler(
            UserManager<User> userManager,
            IJwtService jwtService,
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<LoginResponseDto>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

            if (user == null)
                return Result<LoginResponseDto>.Failure(_localizer["InvalidCredentials"], 400);

            // ================= VERIFICATION CHECK =================
            if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyEmailAndPhone"],
                    403);
            }

            if (!user.EmailConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyEmail"],
                    403);
            }

            if (!user.PhoneNumberConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyPhone"],
                    403);
            }

            // ================= PASSWORD CHECK =================
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
                return Result<LoginResponseDto>.Failure(_localizer["InvalidCredentials"], 400);

            // ================= ROLES =================
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Client";

            // ================= DELIVERY APPROVAL CHECK =================
            if (role == "Delivery")
            {
                var delivery = await _context.Deliveries
                    .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

                if (delivery == null)
                {
                    return Result<LoginResponseDto>.Failure(
                        _localizer["DeliveryProfileNotFound"],
                        403);
                }

                if (!delivery.IsApproved)
                {
                    return Result<LoginResponseDto>.Failure(
                        _localizer["DeliveryNotApproved"],
                        403);
                }
            }

            // ================= PERMISSIONS =================
            var permissions = await _context.RolePermissions
                .Where(rp => rp.Role.Name == role)
                .Select(rp => rp.Permission.PermissionName)
                .ToListAsync(cancellationToken);

            // ================= TOKEN =================
            var token = await _jwtService.GenerateToken(user, role, permissions);

            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireAt = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            // ================= RESPONSE =================
            var response = new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.Id,
                FullName = user.FullName,
                Role = role,
                Permissions = permissions
            };

            return Result<LoginResponseDto>.Success(
                response,
                _localizer["LoginSuccess"],
                200);
        }
    }
}