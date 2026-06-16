using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetAll;

public sealed class GetAllServiceBusinessPlansQueryHandler(
IApplicationDbContext db,
IStringLocalizer localizer)
: IRequestHandler<
GetAllServiceBusinessPlansQuery,
Result<PaginatedList<ServiceBusinessPlanResponse>>>
{
    public async Task<Result<PaginatedList<ServiceBusinessPlanResponse>>> Handle(
    GetAllServiceBusinessPlansQuery request,
    CancellationToken cancellationToken)
    {
        var query = db.ServiceBusinessPlans
        .AsNoTracking()
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x =>
                (x.NameEn != null && x.NameEn.Contains(request.Name)) ||
                (x.NameAr != null && x.NameAr.Contains(request.Name)));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedById))
        {
            query = query.Where(x => x.CreatedById == request.CreatedById);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);
        }

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
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
                x.UpdatedAt));

        var result = await PaginatedList<ServiceBusinessPlanResponse>
            .CreateAsync(
                projected,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<ServiceBusinessPlanResponse>>
            .Success(
                result,
                localizer["ServiceBusinessPlansRetrievedSuccessfully"]);
    }

}
