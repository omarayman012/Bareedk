using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class ApproveDeliveryHandler
       : IRequestHandler<ApproveDeliveryCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ApproveDeliveryHandler> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApproveDeliveryHandler(
            IApplicationDbContext context,
            IStringLocalizer<ApproveDeliveryHandler> localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(
            ApproveDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<string>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= GET DELIVERY =================
            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(x => x.UserId == request.DeliveryId, cancellationToken);

            if (delivery == null)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            // ================= CHECK APPROVED =================
            if (delivery.IsApproved)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryAlreadyApproved"],
                    400);
            }

            // ================= APPROVE =================
            delivery.IsApproved = true;
            delivery.ApprovedAt = DateTime.UtcNow;

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                "Approved Successfully",
                _localizer["DeliveryApprovedSuccessfully"],
                200);
        }
    }
}