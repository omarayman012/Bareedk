namespace BaridikExpress.Application.Features.Customer.Commands.ToggleCustomerStatus;

public sealed record ToggleCustomerStatusCommand(Guid Id)
    : IRequest<Result<bool>>;