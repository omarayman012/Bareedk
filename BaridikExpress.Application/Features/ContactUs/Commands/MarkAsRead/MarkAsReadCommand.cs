namespace BaridikExpress.Application.Features.ContactUs.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(
    Guid Id
) : IRequest<Result<bool>>;