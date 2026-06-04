using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetAll;

public class GetAllGovernmentQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllGovernmentQuery,
        Result<PaginatedList<GovernmentDto>>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<PaginatedList<GovernmentDto>>> Handle(
        GetAllGovernmentQuery request,
        CancellationToken cancellationToken)
    {
        var query = _applicationDb.Governments
     .AsNoTracking()
     .Include(x => x.Country)
     .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim().ToLower();
            query = query.Where(x =>
                x.GovernmentNameAr.ToLower().Contains(name) ||
                x.GovernmentNameEn.ToLower().Contains(name));
        }

        if (request.CountryId.HasValue)
            query = query.Where(x => x.CountryId == request.CountryId.Value);

        query = query.ApplyCommonFilters(request);

        var governmentsQuery = query
            .Select(x => new GovernmentDto
            {
                Id = x.GovernmentId,

                Name = new LocalizedDto
                {
                    AR = x.GovernmentNameAr,
                    EN = x.GovernmentNameEn
                },

                Country = new LocalizedNameDto
                {
                    Id = x.Country!.CountryId,
                    AR = x.Country.CountryNameAr,
                    EN = x.Country.CountryNameEn
                },

                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : "",

                CreatedAt = x.CreatedAt,

                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : x.UpdatedById,

                UpdatedAt = x.UpdatedAt,

                IsActive = x.IsActive
            });

        var paginatedGovernments =
            await PaginatedList<GovernmentDto>.CreateAsync(
                governmentsQuery,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<GovernmentDto>>
            .Success(
                paginatedGovernments,
                _localizer["GovernmentsRetrievedSuccessfully"]);
    }
}