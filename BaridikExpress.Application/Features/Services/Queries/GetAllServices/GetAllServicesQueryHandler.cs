using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllServicesQuery, Result<PaginatedList<ServiceResponse>>>
{
    #region Handle
    public async Task<Result<PaginatedList<ServiceResponse>>> Handle(
        GetAllServicesQuery request,
        CancellationToken cancellationToken)
    {
        #region Build Query
        var query = db.Services.AsNoTracking().AsQueryable();
        #endregion

        #region Filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.NameEn.Contains(request.Name) ||
                x.NameAr.Contains(request.Name));

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.CreatedById))
            query = query.Where(x => x.CreatedById == request.CreatedById);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);
        #endregion

        #region Projection
        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ServiceResponse(
                x.Id,
                new LocalizedDto { EN = x.NameEn, AR = x.NameAr },
                x.Price,
                x.Currency,
                x.ImageUrl,
                x.IsActive,
                x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt));
        #endregion

        #region Paginate
        var result = await PaginatedList<ServiceResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);
        #endregion

        return Result<PaginatedList<ServiceResponse>>.Success(result, localizer["ServicesRetrievedSuccessfully"]);
    }
    #endregion
}