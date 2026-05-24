using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.DeliveryTypes.DTO;

namespace BaridikExpress.Application.Features.DeliveryTypes.Queries.GetAllDeliveryTypes;

public sealed class GetAllDeliveryTypesQuery : BaseFilter, IRequest<Result<PaginatedList<DeliveryTypeResponse>>>
{
    public string? Name { get; set; }
}