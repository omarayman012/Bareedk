using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Vehicles.Queries.GetVehicleById;

public class GetVehicleByIdQueryHandler(
    IGenericRepository<Vehicle> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetVehicleByIdQuery, Result<GetVehicleByIdDto>>
{
    private readonly IGenericRepository<Vehicle> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<GetVehicleByIdDto>> Handle(
        GetVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _repo
            .Query()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Where(x => x.Id == request.Id)
            .Select(x => new GetVehicleByIdDto
            {
                Id = x.Id,
                Name = new LocalizedDto
                {
                    EN = x.NameEn,
                    AR = x.NameAr
                },
                LoadCapacityFrom = x.LoadCapacityFrom,
                LoadCapacityTo = x.LoadCapacityTo,
                PricePerTon = x.PricePerTon,
                TotalPrice = x.IsPriceCalculationEnabled
                    ? x.TotalPrice
                    : 0,
                Currency = x.Currency.ToLocalizedDto(),
                CapacityUnit = new LocalizedDto
                {
                    EN = "Ton",
                    AR = "طن"
                },
                ImageUrl = x.ImageUrl,
                IsPriceCalculationEnabled = x.IsPriceCalculationEnabled,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : "",
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : null,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (vehicle is null)
            return Result<GetVehicleByIdDto>.Failure(
                _localizer["VehicleNotFound"],
                404);

        return Result<GetVehicleByIdDto>.Success(
            vehicle,
            _localizer["OperationCompletedSuccessfully"]);
    }
}