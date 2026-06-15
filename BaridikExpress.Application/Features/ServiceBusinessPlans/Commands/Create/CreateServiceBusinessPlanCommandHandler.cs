using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.ServiceModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Create;

public sealed class CreateServiceBusinessPlanCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateServiceBusinessPlanCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateServiceBusinessPlanCommand request,
        CancellationToken cancellationToken)
    {
        #region Normalize Names

        var (nameAr, nameEn) = NormalizeHelper.Normalize(request.NameAr, request.NameEn);
        var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(request.DescriptionAr, request.DescriptionEn);

        #endregion

        #region Validate Uniqueness

        var nameExists = await db.ServiceBusinessPlans
            .AnyAsync(x =>
                x.NameEn == nameEn ||
                x.NameAr == nameAr,
                cancellationToken);

        if (nameExists)
            return Result<Guid>.Failure(localizer["ServiceBusinessPlanAlreadyExists"]);

        #endregion

        #region Upload SVG Image

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}.svg";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "service-business-plan-images");

            if (imageUrl is null)
                return Result<Guid>.Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Create & Save

        var plan = ServiceBusinessPlan.Create(
            nameEn,
            nameAr,
            descriptionEn,
            descriptionAr,
            imageUrl);

        await db.ServiceBusinessPlans.AddAsync(plan, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<Guid>.Success(plan.Id, localizer["ServiceBusinessPlanCreatedSuccessfully"], 201);
    }
}