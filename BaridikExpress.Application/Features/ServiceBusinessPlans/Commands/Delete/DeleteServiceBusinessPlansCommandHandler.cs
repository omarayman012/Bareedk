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
        if (request.Ids is null || request.Ids.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlansIdsRequired"],
                400);
        }

        var ids = request.Ids.Distinct().ToList();

        var plans = await db.ServiceBusinessPlans
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (plans.Count == 0)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlansNotFound"],
                404);
        }

        var existingPlanIds = plans.Select(x => x.Id).ToList();

        var hasChildren = await db.ServiceBusinessPlans
            .AsNoTracking()
            .AnyAsync(x => existingPlanIds.Contains(x.Id), cancellationToken);

        if (hasChildren)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlansCannotBeDeletedBecauseItHasChildren"],
                400);
        }

        db.ServiceBusinessPlans.RemoveRange(plans);

        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            localizer["ServiceBusinessPlansDeletedSuccessfully"]);
    }
}