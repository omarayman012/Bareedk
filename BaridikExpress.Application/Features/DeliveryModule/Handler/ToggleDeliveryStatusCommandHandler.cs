using BaridikExpress.Application.Features.DeliveryModule.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class ToggleDeliveryStatusCommandHandler
        : IRequestHandler<ToggleDeliveryStatusCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToggleDeliveryStatusCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(
            ToggleDeliveryStatusCommand request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity?.IsAuthenticated != true)
            {
                return Result<string>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId,
                    cancellationToken);

            if (delivery == null)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            delivery.Active = !delivery.Active;

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                delivery.Active
                    ? _localizer["DeliveryActivated"]
                    : _localizer["DeliveryDeactivated"],
                _localizer["Success"],
                200);
        }
    }
}