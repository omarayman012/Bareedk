using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.OurPlans.DTO;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Application.Features.OurPlans.Queries.GetAllPlans;

public sealed class GetAllPlansQueryHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllPlansQuery, Result<PaginatedList<GetAllPlansDto>>>
{
    public async Task<Result<PaginatedList<GetAllPlansDto>>> Handle(
        GetAllPlansQuery request,
        CancellationToken cancellationToken)
    {
        var query = repo.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.NameEn.Contains(request.Name) ||
                x.NameAr.Contains(request.Name));

        query = query.ApplyCommonFilters(request);

        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllPlansDto
            {
                Id = x.Id,

                Name = new LocalizedDto
                {
                    EN = x.NameEn,
                    AR = x.NameAr
                },

                Type = x.Type,

                Description = new LocalizedDto
                {
                    EN = x.DescriptionEn,
                    AR = x.DescriptionAr
                },

                Features = new LocalizedListDto
                {
                    AR = x.FeaturesAr,
                    EN = x.FeaturesEn
                },

                IsActive = x.IsActive,

                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : "",

                CreatedAt = x.CreatedAt,

                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : "",

                UpdatedAt = x.UpdatedAt
            });

        var paginatedResult = await PaginatedList<GetAllPlansDto>
            .CreateAsync(
                result,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<GetAllPlansDto>>
            .Success(
                paginatedResult,
                localizer["OperationCompletedSuccessfully"]);
    }
}