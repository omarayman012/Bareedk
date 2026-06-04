using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Handler
{
    public class DeletePublishingHouseHandler
      : IRequestHandler<DeletePublishingHouseCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public DeletePublishingHouseHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            DeletePublishingHouseCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.Ids == null || !request.Ids.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["IdsRequired"],
                        400);
                }

                var entities = await _context.PublishingHouses
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                if (!entities.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["NoDataFound"],
                        404);
                }

                _context.PublishingHouses.RemoveRange(entities);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    _localizer["DeletedSuccessfully"],
                    200);
            }
            catch (Exception ex)
            {
                return Result<bool>.Error(
                    _localizer["FailedToDelete", ex.Message],
                    500);
            }
        }
    }
}
