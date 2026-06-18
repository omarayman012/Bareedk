using BaridikExpress.Application.Features.DeliveryModule.Command;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class DeleteDeliveryByAdminHandler
     : IRequestHandler<DeleteDeliveryByAdminCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public DeleteDeliveryByAdminHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            DeleteDeliveryByAdminCommand request,
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

            var user = await _userManager.FindByIdAsync(delivery.UserId);

            _context.Deliveries.Remove(delivery);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return Result<string>.Failure(
                        string.Join(", ", result.Errors.Select(x => x.Description)),
                        400);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(
                _localizer["DeliveryDeletedSuccessfully"],
                _localizer["Success"],
                200);
        }
    }
}
