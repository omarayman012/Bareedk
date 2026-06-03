using BaridikExpress.Application.Features.DeliveryModule.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class DeleteDeliveryHandler
         : IRequestHandler<DeleteDeliveryCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteDeliveryHandler(
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
            DeleteDeliveryCommand request,
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

            // ================= GET DELIVERY =================
            var delivery = await _context.Deliveries
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.Id == request.DeliveryId,
                    cancellationToken);

            if (delivery == null)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            // ================= PASSWORD VALIDATION =================
            var passwordValid = await _userManager
                .CheckPasswordAsync(delivery.User, request.Password);

            if (!passwordValid)
            {
                return Result<string>.Failure(
                    _localizer["InvalidPassword"],
                    400);
            }

            // ================= DELETE =================
            _context.Deliveries.Remove(delivery);

            var deleteUserResult = await _userManager.DeleteAsync(delivery.User);

            if (!deleteUserResult.Succeeded)
            {
                return Result<string>.Failure(
                    _localizer["DeleteFailed"],
                    400);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                _localizer["DeliveryDeletedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}