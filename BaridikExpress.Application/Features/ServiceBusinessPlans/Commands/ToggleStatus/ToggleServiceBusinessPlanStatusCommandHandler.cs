using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.ToggleStatus;

public sealed class ToggleServiceBusinessPlanStatusCommandHandler(
IApplicationDbContext db,
IStringLocalizer localizer)
: IRequestHandler<ToggleServiceBusinessPlanStatusCommand, Result<bool>>
{
#region Handle


public async Task<Result<bool>> Handle(
    ToggleServiceBusinessPlanStatusCommand request,
    CancellationToken cancellationToken)
    {
        #region Fetch Service Business Plan

        var plan = await db.ServiceBusinessPlans
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (plan is null)
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlanNotFound"],
                404);

        #endregion

        #region Toggle & Save

        plan.ToggleStatus();

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        var message = plan.IsActive
            ? localizer["ServiceBusinessPlanActivatedSuccessfully"]
            : localizer["ServiceBusinessPlanDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }

#endregion


}
