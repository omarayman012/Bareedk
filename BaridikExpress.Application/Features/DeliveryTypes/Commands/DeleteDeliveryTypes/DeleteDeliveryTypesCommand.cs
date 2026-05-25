namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.DeleteDeliveryTypes;

public sealed record DeleteDeliveryTypesCommand(List<Guid> Ids)
    : IRequest<Result<bool>>;