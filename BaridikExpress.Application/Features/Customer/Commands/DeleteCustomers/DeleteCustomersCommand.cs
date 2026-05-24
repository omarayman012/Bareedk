namespace BaridikExpress.Application.Features.Customer.Commands.DeleteCustomers;

public sealed record DeleteCustomersCommand(List<Guid> Ids)
    : IRequest<Result<bool>>;