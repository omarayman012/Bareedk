using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Vehicles.DTO;

namespace BaridikExpress.Application.Features.Vehicles.Queries.GetAllVehicles
{
        public class GetAllVehiclesQuery
            : BaseFilter,
              IRequest<Result<PaginatedList<GetAllVehiclesDto>>>
        {
            public string? Name { get; set; }
            public bool? IsPriceCalculationEnabled { get; set; }
        }
    
}