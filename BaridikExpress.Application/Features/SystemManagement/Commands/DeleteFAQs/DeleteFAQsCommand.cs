namespace BaridikExpress.Application.Features.SystemManagement.Commands.DeleteFAQs;

public sealed record DeleteFAQsCommand(
    List<Guid> Ids
) : IRequest<Result<bool>>;