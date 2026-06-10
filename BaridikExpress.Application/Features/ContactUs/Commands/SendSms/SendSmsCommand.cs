namespace BaridikExpress.Application.Features.ContactUs.Commands.SendSms;

public sealed record SendSmsCommand(
  string PhoneNumber,
  string Message
) : IRequest<Result<bool>>;
