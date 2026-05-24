using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class ApproveDeliveryHandler
       : IRequestHandler<ApproveDeliveryCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ApproveDeliveryHandler> _localizer;

        public ApproveDeliveryHandler(
            IApplicationDbContext context,
            IStringLocalizer<ApproveDeliveryHandler> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            ApproveDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(x => x.Id == request.DeliveryId, cancellationToken);

            if (delivery == null)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            if (delivery.IsApproved)
            {
                return Result<string>.Failure(
                    _localizer["DeliveryAlreadyApproved"],
                    400);
            }

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
