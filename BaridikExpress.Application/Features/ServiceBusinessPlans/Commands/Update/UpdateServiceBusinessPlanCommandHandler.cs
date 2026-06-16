using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Update;

public sealed class UpdateServiceBusinessPlanCommandHandler(
IApplicationDbContext db,
IStringLocalizer localizer,
IFileStorageService fileStorage)
: IRequestHandler<UpdateServiceBusinessPlanCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
    UpdateServiceBusinessPlanCommand request,
    CancellationToken cancellationToken)
    {
        var plan = await db.ServiceBusinessPlans
        .FirstOrDefaultAsync(
        x => x.Id == request.Id,
        cancellationToken);

    if (plan is null)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlanNotFound"],
                404);
        }

        var nameEn = string.IsNullOrWhiteSpace(request.NameEn)
            ? null
            : request.NameEn.Trim();

        var nameAr = string.IsNullOrWhiteSpace(request.NameAr)
            ? null
            : request.NameAr.Trim();

        var descriptionEn = string.IsNullOrWhiteSpace(request.DescriptionEn)
            ? null
            : request.DescriptionEn.Trim();

        var descriptionAr = string.IsNullOrWhiteSpace(request.DescriptionAr)
            ? null
            : request.DescriptionAr.Trim();

        var exists = await db.ServiceBusinessPlans
            .AnyAsync(x =>
                x.Id != request.Id &&
                (
                    (nameEn != null && x.NameEn == nameEn) ||
                    (nameAr != null && x.NameAr == nameAr)
                ),
                cancellationToken);

        if (exists)
        {
            return Result<bool>.Failure(
                localizer["ServiceBusinessPlanAlreadyExists"]);
        }

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var fileName = $"{Guid.NewGuid()}.svg";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                fileName,
                "service-business-plan-images");

            if (imageUrl is null)
            {
                return Result<bool>.Failure(
                    localizer["ImageUploadFailed"],
                    400);
            }
        }

        plan.Update(
            nameEn,
            nameAr,
            descriptionEn,
            descriptionAr,
            imageUrl);

        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            localizer["ServiceBusinessPlanUpdatedSuccessfully"]);
    }

}
