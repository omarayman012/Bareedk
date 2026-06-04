using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;
using BaridikExpress.Domain.Enum;


namespace BaridikExpress.Application.Features.Vehicles.Queries.GetAllVehicles
{
    public class GetAllVehiclesQueryHandler(
        IGenericRepository<Vehicle> repo,
        IStringLocalizer localizer
    ) : IRequestHandler< GetAllVehiclesQuery,
            Result<PaginatedList<GetAllVehiclesDto>>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<PaginatedList<GetAllVehiclesDto>>> Handle(
            GetAllVehiclesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repo.Query();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var (nameAr, nameEn) =
                    NormalizeHelper.Normalize(
                        request.Name,
                        request.Name);

                query = query.Where(x =>
                    x.NameEn.Contains(nameEn) ||
                    x.NameAr.Contains(nameAr));
            }

            if (request.IsPriceCalculationEnabled.HasValue)
            {
                query = query.Where(x =>
                    x.IsPriceCalculationEnabled ==
                    request.IsPriceCalculationEnabled.Value);
            }

            query = query.ApplyCommonFilters(request);
                var result = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new GetAllVehiclesDto
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
                TotalPrice = x.IsPriceCalculationEnabled
                    ? x.TotalPrice
                    : 0,
                   Currency = x.Currency.ToLocalizedDto(),
                    CapacityUnit = new LocalizedDto
                {
                    EN = "Ton",
                    AR = "طن"
                },
                CreatedBy = x.CreatedBy!.FullName,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : null,
                UpdatedAt = x.UpdatedAt,
                IsPriceCalculationEnabled =
                    x.IsPriceCalculationEnabled,
                IsActive = x.IsActive
                });

            var paginatedResult =
                await PaginatedList<GetAllVehiclesDto>
                    .CreateAsync(
                        result,
                        request.PageNumber,
                        request.PageSize);

            return Result<PaginatedList<GetAllVehiclesDto>>
                .Success(
                    paginatedResult,
                    _localizer["OperationCompletedSuccessfully"]);
        }
    }
}
