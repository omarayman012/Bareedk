namespace BaridikExpress.Application.Features.ContactUs.Commands.DeleteContactUs;

public sealed record DeleteContactUsCommand(
    List<Guid> Ids
) : IRequest<Result<bool>>;