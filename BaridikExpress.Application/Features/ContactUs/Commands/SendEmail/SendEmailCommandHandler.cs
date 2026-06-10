using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendEmail
{
    public sealed class SendEmailCommandHandler(
     IEmailService emailService,
     IStringLocalizer localizer)
     : IRequestHandler<SendEmailCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            SendEmailCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Attachment is not null)
                await emailService.SendEmailWithAttachmentAsync(
                    request.Email,
                    request.Subject,
                    request.Message,
                    request.Attachment);
            else
                await emailService.SendEmailAsync(
                    request.Email,
                    request.Subject,
                    request.Message);

            return Result<bool>.Success(true, localizer["EmailSentSuccessfully"]);
        }
    }
}
