using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Vehicles.DTO;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Vehicles.Queries.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllVehiclesQuery, Result<PaginatedList<GetAllVehiclesDto>>>
{
    public async Task<Result<PaginatedList<GetAllVehiclesDto>>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.Vehicles.AsNoTracking().AsQueryable();

        #region Filters

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.NameEn.Contains(request.Name) ||
                x.NameAr.Contains(request.Name));

        if (request.IsPriceCalculationEnabled.HasValue)
            query = query.Where(x =>
                x.IsPriceCalculationEnabled == request.IsPriceCalculationEnabled.Value);

        query = query.ApplyCommonFilters(request);

        #endregion

        #region Projection

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllVehiclesDto
            {
                Id = x.Id,
                Name = new LocalizedDto { EN = x.NameEn, AR = x.NameAr },
                ImageUrl = x.ImageUrl,
                LoadCapacityFrom = $"{x.LoadCapacityFrom} Tons",
                LoadCapacityTo = $"{x.LoadCapacityTo} Tons",
                PricePerTon = $"{x.PricePerTon} SAR/Ton",
                TotalPrice = x.IsPriceCalculationEnabled
                    ? $"{x.PricePerTon} * {x.LoadCapacityTo} = {x.PricePerTon * x.LoadCapacityTo} SAR"
                    : "0",
                IsPriceCalculationEnabled = x.IsPriceCalculationEnabled,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : null,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                UpdatedAt = x.UpdatedAt,
            });

        #endregion

        #region Paginate

        var result = await PaginatedList<GetAllVehiclesDto>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        #endregion

        return Result<PaginatedList<GetAllVehiclesDto>>
            .Success(result, localizer["OperationCompletedSuccessfully"]);
    }
}