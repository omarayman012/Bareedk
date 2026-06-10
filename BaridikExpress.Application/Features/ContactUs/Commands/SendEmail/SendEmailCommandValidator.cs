using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendEmail
{
    public sealed class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Subject)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(5000);

            RuleFor(x => x.Attachment)
                .Must(file => file!.Length <= 10 * 1024 * 1024)
                .When(x => x.Attachment is not null)
                .WithMessage("File size must not exceed 10 MB");
        }
    }
}
