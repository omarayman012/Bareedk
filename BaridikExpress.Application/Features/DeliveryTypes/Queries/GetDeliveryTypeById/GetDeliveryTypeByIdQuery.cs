using BaridikExpress.Application.Features.DeliveryTypes.DTO;

namespace BaridikExpress.Application.Features.DeliveryTypes.Queries.GetDeliveryTypeById;

public sealed record GetDeliveryTypeByIdQuery(Guid Id)
    : IRequest<Result<DeliveryTypeResponse>>;