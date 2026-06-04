using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        var query = db.Vehicles
            .AsNoTracking()
            .ApplyCommonFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.NameEn.Contains(request.Name) ||
                x.NameAr.Contains(request.Name));

        if (request.IsPriceCalculationEnabled.HasValue)
            query = query.Where(x =>
                x.IsPriceCalculationEnabled == request.IsPriceCalculationEnabled.Value);

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(Projection);

        var result = await PaginatedList<GetAllVehiclesDto>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        return Result<PaginatedList<GetAllVehiclesDto>>
            .Success(result, localizer["OperationCompletedSuccessfully"]);
    }

    private static readonly Expression<Func<Vehicle, GetAllVehiclesDto>> Projection = x =>
        new GetAllVehiclesDto
        {
            Id = x.Id,
            Name = new LocalizedDto
            {
                EN = x.NameEn,
                AR = x.NameAr
            },
            ImageUrl = x.ImageUrl,
            LoadCapacityFrom = x.LoadCapacityFrom,
            LoadCapacityTo = x.LoadCapacityTo,
            PricePerTon = x.PricePerTon,
            TotalPrice = x.IsPriceCalculationEnabled ? x.TotalPrice : 0,
            Currency = x.Currency.ToLocalizedDto(),
            CapacityUnit = new LocalizedDto
            {
                EN = "Ton",
                AR = "طن"
            },
            IsPriceCalculationEnabled = x.IsPriceCalculationEnabled,
            IsActive = x.IsActive,
            CreatedBy = x.CreatedBy != null
                ? x.CreatedBy.FullName
                : "",
            CreatedAt = x.CreatedAt,
            UpdatedBy = x.UpdatedBy != null
                ? x.UpdatedBy.FullName
                : "",
            UpdatedAt = x.UpdatedAt
        };
}