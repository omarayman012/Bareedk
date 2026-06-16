using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetById;

public sealed class GetServiceBusinessPlanByIdQueryHandler(
IApplicationDbContext db,
IStringLocalizer localizer)
: IRequestHandler<GetServiceBusinessPlanByIdQuery, Result<ServiceBusinessPlanResponse>>
{
    public async Task<Result<ServiceBusinessPlanResponse>> Handle(
    GetServiceBusinessPlanByIdQuery request,
    CancellationToken cancellationToken)
    {
        var plan = await db.ServiceBusinessPlans
        .AsNoTracking()
        .Where(x => x.Id == request.Id)
        .Select(x => new ServiceBusinessPlanResponse(
        x.Id,
        new LocalizedDto
        {
            EN = x.NameEn,
            AR = x.NameAr
        },
        new LocalizedDto
        {
            EN = x.DescriptionEn,
            AR = x.DescriptionAr
        },
        x.ImageUrl,
        x.IsActive,
        x.CreatedBy != null ? x.CreatedBy.FullName : null,
        x.CreatedAt,
        x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
        x.UpdatedAt))
        .FirstOrDefaultAsync(cancellationToken);

    if (plan is null)
        {
            return Result<ServiceBusinessPlanResponse>
                .Failure(localizer["ServiceBusinessPlanNotFound"], 404);
        }

        return Result<ServiceBusinessPlanResponse>
            .Success(plan, localizer["ServiceBusinessPlanRetrievedSuccessfully"]);
    }

}
