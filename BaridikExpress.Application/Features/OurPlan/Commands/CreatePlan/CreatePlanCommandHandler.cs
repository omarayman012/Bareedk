using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Application.Features.OurPlans.Commands.CreatePlan;

public sealed class CreatePlanCommandHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<CreatePlanCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreatePlanCommand request,
        CancellationToken cancellationToken)
    {
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
            .AnyAsync(
                x => x.NameAr == nameAr ||
                     x.NameEn == nameEn,
                cancellationToken);

        if (isExists)
        {
            return Result<Guid>.Failure(
                localizer["PlanAlreadyExists"],
                409);
        }

        #endregion

        #region Normalize Features

        var featuresAr = new List<string>();
        var featuresEn = new List<string>();

        var features = request.FeaturesAr?.Any() == true
            ? request.FeaturesAr
            : request.FeaturesEn;

        foreach (var feature in features)
        {
            var (featureAr, featureEn) =
                NormalizeHelper.Normalize(
                    feature,
                    feature);

            featuresAr.Add(featureAr);
            featuresEn.Add(featureEn);
        }

        #endregion

        #region Create & Save

        var plan = new Plan(
            nameEn,
            nameAr,
             request.Type,
            featuresEn,
            featuresAr,
            descriptionEn,
            descriptionAr);

        await repo.AddAsync(
            plan,
            cancellationToken);

        #endregion

        return Result<Guid>.Success(
            plan.Id,
            localizer["PlanCreatedSuccessfully"],
            201);
    }
}