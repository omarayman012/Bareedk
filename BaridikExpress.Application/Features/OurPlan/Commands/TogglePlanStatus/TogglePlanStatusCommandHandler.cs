using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Application.Features.OurPlans.Commands.TogglePlanStatus;

public sealed class TogglePlanStatusCommandHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<TogglePlanStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        TogglePlanStatusCommand request,
        CancellationToken cancellationToken)
    {
        #region Get Plan

        var plan = await repo.Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (plan is null)
            return Result<bool>.Failure(
                localizer["PlanNotFound"],
                404);
        #endregion

        #region Toggle Status
        plan.ToggleStatus();
        await repo.UpdateAsync(plan);
        #endregion

        return Result<bool>.Success(
                true,
                 localizer["OperationCompletedSuccessfully"]
             );
    }
}