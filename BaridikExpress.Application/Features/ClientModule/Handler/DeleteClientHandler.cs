using BaridikExpress.Application.Features.ClientModule.Commond;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.ClientModule.Handler
{
    public class DeleteClientHandler
         : IRequestHandler<DeleteClientCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteClientHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(
            DeleteClientCommand request,
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

            // ================= PASSWORD CONFIRM =================
            if (request.Password != request.ConfirmPassword)
            {
                return Result<string>.Failure(
                    _localizer["PasswordAndConfirmPasswordNotMatch"],
                    400);
            }

            // ================= GET CLIENT =================
            var client = await _context.Clients
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId,
                    cancellationToken);

            if (client == null)
            {
                return Result<string>.Failure(
                    _localizer["ClientNotFound"],
                    404);
            }

            // ================= PASSWORD CHECK =================
            var isValidPassword = await _userManager
                .CheckPasswordAsync(client.User, request.Password);

            if (!isValidPassword)
            {
                return Result<string>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= DELETE CLIENT =================
            _context.Clients.Remove(client);

            var deleteUserResult = await _userManager.DeleteAsync(client.User);

            if (!deleteUserResult.Succeeded)
            {
                return Result<string>.Failure(
                    _localizer["DeleteFailed"],
                    400);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                _localizer["ClientDeletedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}