namespace BaridikExpress.Application.Features.ContactUs.Commands.SendEmail;

public sealed record SendEmailCommand(
 string Email,
 string Subject,
 string Message,
 IFormFile? Attachment
) : IRequest<Result<bool>>;
