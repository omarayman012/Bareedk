namespace BaridikExpress.Application.Features.ContactUs.Commands.MarkAllAsRead;

public sealed record MarkAsReadBulkCommand(List<Guid> Ids)
    : IRequest<Result<bool>>;