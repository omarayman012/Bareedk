using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.OurPlans.Commands.UpdatePlan;

public sealed class UpdatePlanCommandHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<UpdatePlanCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        UpdatePlanCommand request,
        CancellationToken cancellationToken)
    {
        #region Get Plan

        var plan = await repo.Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (plan is null)
            return Result<Guid>.Failure(
                localizer["PlanNotFound"],
                404);

        #endregion

        #region Normalize Names & Descriptions

        var (nameAr, nameEn) =
            NormalizeHelper.Normalize(
                request.NameAr,
                request.NameEn);

        var (descriptionAr, descriptionEn) =
            NormalizeHelper.Normalize(
                request.DescriptionAr,
                request.DescriptionEn);

        #endregion
        #region Validate Name Uniqueness

        var isExists = await repo.Query()
          .AnyAsync(x =>
              x.Id != request.Id &&
              (
                  x.NameAr == nameAr ||
                  x.NameEn == nameEn
              ),
              cancellationToken);

        if (isExists)
        {
            return Result<Guid>.Failure(
                localizer["PlanAlreadyExists"],
                409);
        }

        #endregion


        #region Normalize Features

        List<string>? featuresAr = null;
        List<string>? featuresEn = null;

        if (request.FeaturesAr is not null || request.FeaturesEn is not null)
        {
            var features = request.FeaturesAr?.Any() == true
                ? request.FeaturesAr
                : request.FeaturesEn ?? [];

            featuresAr = [];
            featuresEn = [];

            foreach (var feature in features)
            {
                var (featureAr, featureEn) =
                    NormalizeHelper.Normalize(feature, feature);

                featuresAr.Add(featureAr);
                featuresEn.Add(featureEn);
            }
        }
        #endregion

        #region Update
        plan.Update(
            nameEn,
            nameAr,
            request.Type,
            featuresEn,
            featuresAr,
            descriptionEn,
            descriptionAr);
        await repo.UpdateAsync(plan);
        #endregion
        return Result<Guid>.Success(
            plan.Id,
            localizer["PlanUpdatedSuccessfully"]);
    }
}