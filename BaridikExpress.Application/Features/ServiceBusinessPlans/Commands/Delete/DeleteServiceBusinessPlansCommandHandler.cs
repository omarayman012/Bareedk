using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Delete;

public sealed class DeleteServiceBusinessPlansCommandHandler(
IApplicationDbContext db,
IStringLocalizer localizer)
: IRequestHandler<DeleteServiceBusinessPlansCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
    DeleteServiceBusinessPlansCommand request,
    CancellationToken cancellationToken)
    {
        var plans = await db.ServiceBusinessPlans
        .Where(x => request.Ids.Contains(x.Id))
        .ToListAsync(cancellationToken);

    if (plans.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlansNotFound"],
                404);
        }

        db.ServiceBusinessPlans.RemoveRange(plans);

        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            localizer["ServiceBusinessPlansDeletedSuccessfully"]);
    }

}
