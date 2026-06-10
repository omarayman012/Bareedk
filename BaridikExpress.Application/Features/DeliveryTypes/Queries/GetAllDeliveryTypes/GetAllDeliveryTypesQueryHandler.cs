using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.DeliveryTypes.DTO;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.DeliveryTypes.Queries.GetAllDeliveryTypes;

public sealed class GetAllDeliveryTypesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllDeliveryTypesQuery, Result<PaginatedList<DeliveryTypeResponse>>>
{
    #region Handle

    public async Task<Result<PaginatedList<DeliveryTypeResponse>>> Handle(
        GetAllDeliveryTypesQuery request,
        CancellationToken cancellationToken)
    {
        #region Build Query

        var query = db.DeliveryTypes
            .AsNoTracking()
            .AsQueryable();

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
            .Select(x => new DeliveryTypeResponse(
                       x.Id,
                       new LocalizedDto { EN = x.NameEn, AR = x.NameAr },
                       x.DaysFrom,
                       x.DaysTo,
                       x.PricePerShipment,
                       x.DaysTo * x.PricePerShipment,
                       x.Currency.ToString(),
                       x.IsSwitchActive,
                       x.IsActive,
                       x.ImageUrl,
                       new LocalizedDto { EN = x.DescriptionEn, AR = x.DescriptionAr },
                       x.CreatedBy != null ? x.CreatedBy.FullName : null,
                       x.CreatedAt,
                       x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                       x.UpdatedAt));

        #endregion

        #region Paginate

        var result = await PaginatedList<DeliveryTypeResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        #endregion

        return Result<PaginatedList<DeliveryTypeResponse>>
            .Success(result, localizer["DeliveryTypesRetrievedSuccessfully"]);
    }

    #endregion
}