namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.ToggleDeliveryTypeStatus;

public sealed record ToggleDeliveryTypeStatusCommand(Guid Id)
    : IRequest<Result<bool>>;