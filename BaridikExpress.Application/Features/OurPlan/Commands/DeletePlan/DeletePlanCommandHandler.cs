using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.OurPlans.Commands.DeletePlan;

public sealed class DeletePlanCommandHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<DeletePlanCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeletePlanCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Input

        if (request.Ids is null || !request.Ids.Any())
            return Result<bool>.Failure(
                localizer["InvalidIds"],
                400);

        #endregion

        #region Fetch Plans

        var plans = await repo.Query()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!plans.Any())
            return Result<bool>.Failure(
                localizer["PlansNotFound"],
                404);

        if (plans.Count != request.Ids.Count)
            return Result<bool>.Failure(
                localizer["SomePlansNotFound"],
                404);

        #endregion

        #region Validate Customers

        var hasCustomers = plans.Any(x => x.Customers.Any());

        if (hasCustomers)
            return Result<bool>.Failure(
                localizer["PlanHasCustomers"],
                400);

        #endregion

        #region Delete Plans

        await repo.DeleteRangeAsync(plans);

        #endregion

        return Result<bool>.Success(
            true,
            localizer["PlansDeletedSuccessfully"]);
    }
}