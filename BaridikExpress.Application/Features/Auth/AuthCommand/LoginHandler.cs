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
            var loginValue = request.EmailOrPhone.Trim();

            var normalizedEmail = loginValue.ToUpper();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x =>
                    x.NormalizedEmail == normalizedEmail ||
                    x.PhoneNumber == loginValue,
                    cancellationToken);

            if (user == null)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["InvalidCredentials"],
                    400);
            }

            // ================= LOGIN TYPE =================

            bool loginByEmail =
                !string.IsNullOrWhiteSpace(user.Email) &&
                user.Email.Equals(loginValue, StringComparison.OrdinalIgnoreCase);

            bool loginByPhone =
                !string.IsNullOrWhiteSpace(user.PhoneNumber) &&
                user.PhoneNumber == loginValue;

            // ================= VERIFICATION CHECK =================

            if (!user.EmailConfirmed && !user.PhoneNumberConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyEmailAndPhone"],
                    403);
            }

            if (loginByEmail && !user.EmailConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyEmail"],
                    403);
            }

            if (loginByPhone && !user.PhoneNumberConfirmed)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["VerifyPhone"],
                    403);
            }

            // ================= PASSWORD CHECK =================

            var passwordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.Password);

            if (!passwordValid)
            {
                return Result<LoginResponseDto>.Failure(
                    _localizer["InvalidCredentials"],
                    400);
            }

            // ================= ROLES =================

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault() ?? "Client";

            // ================= DELIVERY CHECK =================

            if (role == "Delivery")
            {
                var delivery = await _context.Deliveries
                    .FirstOrDefaultAsync(
                        x => x.UserId == user.Id,
                        cancellationToken);

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
                .Where(x => x.Role.Name == role)
                .Select(x => x.Permission.PermissionName)
                .ToListAsync(cancellationToken);

            // ================= TOKEN =================

            var token =
                await _jwtService.GenerateToken(
                    user,
                    role,
                    permissions);

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