using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Delete
{
    public sealed class DeleteTalkServicesCommandHandler(
        IApplicationDbContext db,
        IStringLocalizer localizer)
        : IRequestHandler<DeleteTalkServicesCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            DeleteTalkServicesCommand request,
            CancellationToken cancellationToken)
        {
            var ids = request.Ids.Distinct().ToList();

            var talkServices = await db.TalkServices
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (talkServices.Count == 0)
                return Result<bool>.Failure(localizer["TalkServiceNotFound"]);

            db.TalkServices.RemoveRange(talkServices);

            await db.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, localizer["TalkServiceDeletedSuccessfully"]);
        }
    }
}
