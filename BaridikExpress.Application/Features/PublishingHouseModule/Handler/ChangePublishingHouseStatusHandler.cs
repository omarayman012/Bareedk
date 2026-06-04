using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Handler
{

    public class ChangePublishingHouseStatusHandler
       : IRequestHandler<ChangePublishingHouseStatusCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IGetCurrentUserRepository _currentUser;

        public ChangePublishingHouseStatusHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IGetCurrentUserRepository currentUser)
        {
            _context = context;
            _localizer = localizer;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            ChangePublishingHouseStatusCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var userId = _currentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    return Result<string>.Failure(
                        _localizer["Unauthorized"],
                        401);

                var publishingHouse = await _context.PublishingHouses
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (publishingHouse == null)
                    return Result<string>.Failure(
                        _localizer["PublishingHouseNotFound"],
                        404);

                publishingHouse.Active = !publishingHouse.Active;

                publishingHouse.UpdatedAt = DateTime.UtcNow;
                publishingHouse.UpdatedById = userId;

                await _context.SaveChangesAsync(cancellationToken);

                var message = publishingHouse.Active
                    ? _localizer["PublishingHouseActivatedSuccessfully"]
                    : _localizer["PublishingHouseDeactivatedSuccessfully"];

                return Result<string>.Success(
                    message,
                    message,
                    200);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(
                    _localizer["FailedToChangePublishingHouseStatus", ex.Message],
                    500);
            }
        }
    }
}