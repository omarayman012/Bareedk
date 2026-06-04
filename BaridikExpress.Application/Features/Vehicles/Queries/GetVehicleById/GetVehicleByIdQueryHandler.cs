using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;
using ServiceStack;

namespace BaridikExpress.Application.Features.Vehicles.Queries.GetVehicleById
{
    public class GetVehicleByIdQueryHandler(
        IGenericRepository<Vehicle> repo,
        IStringLocalizer localizer
    ) : IRequestHandler<GetVehicleByIdQuery, Result<GetVehicleByIdDto>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<GetVehicleByIdDto>> Handle(
            GetVehicleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var vehicle = await _repo.GetByIdAsync(request.Id);

            if (vehicle is null)
                return Result<GetVehicleByIdDto>.Failure(
                    _localizer["VehicleNotFound"]);

            var response = new GetVehicleByIdDto
            {
                Id = vehicle.Id,
                Name = new LocalizedDto
                {
                    EN = vehicle.NameEn,
                    AR = vehicle.NameAr
                },
                LoadCapacityFrom = vehicle.LoadCapacityFrom,
                LoadCapacityTo = vehicle.LoadCapacityTo,
                PricePerTon = vehicle.PricePerTon,
                TotalPrice = vehicle.IsPriceCalculationEnabled
                    ? vehicle.TotalPrice
                    : 0,
                Currency = vehicle.Currency.ToLocalizedDto(),
                CapacityUnit = new LocalizedDto
                {
                    EN = "Ton",
                    AR = "??"
                },
                ImageUrl = vehicle.ImageUrl,
                IsPriceCalculationEnabled = vehicle.IsPriceCalculationEnabled,
                IsActive = vehicle.IsActive
            };

            return Result<GetVehicleByIdDto>.Success(
                response,
                _localizer["OperationCompletedSuccessfully"]);
        }
    }
}