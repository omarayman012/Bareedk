using BaridikExpress.Application.Features.DeliveryModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class RejectDeliveryHandler
     : IRequestHandler<RejectDeliveryCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RejectDeliveryHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(
            RejectDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<string>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(
                    x => x.UserId == request.DeliveryId,
                    cancellationToken);

            if (delivery == null)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            if (!delivery.IsApproved)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryAlreadyRejected"],
                    400);
            }

            delivery.IsApproved = false;
            delivery.ApprovedAt = null;

            _context.Deliveries.Update(delivery);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                _localizer["DeliveryRejectedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}
