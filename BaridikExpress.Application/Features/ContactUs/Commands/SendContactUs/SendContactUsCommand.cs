namespace BaridikExpress.Application.Features.ContactUs.Commands.SendContactUs;

public sealed record SendContactUsCommand(
    string Name,
    string Email,
    string Phone,
    string Message
) : IRequest<Result<bool>>;