using BaridikExpress.Application.Features.Auth.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.AuthHandler
{
    public class ChangePasswordHandler
        : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordHandler(
            UserManager<User> userManager,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var currentUser = _httpContextAccessor.HttpContext?.User;

            if (currentUser == null || !currentUser.Identity?.IsAuthenticated == true)
            {
                return Result<string>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= CONFIRM PASSWORD CHECK =================
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result<string>.Failure(
                    _localizer["PasswordAndConfirmPasswordNotMatch"],
                    400);
            }

            // ================= GET USER =================
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.Id == request.UserId,
                    cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure(
                    _localizer["UserNotFound"],
                    404);
            }

            // ================= CHANGE PASSWORD =================
            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(x => x.Description));

                return Result<string>.Failure(
                    errors,
                    400);
            }

            return Result<string>.Success(
                _localizer["PasswordChangedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}