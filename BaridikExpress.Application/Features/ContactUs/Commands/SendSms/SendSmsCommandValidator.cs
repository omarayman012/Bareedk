using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendSms
{
    public sealed class SendSmsCommandValidator : AbstractValidator<SendSmsCommand>
    {
        public SendSmsCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .WithMessage("Invalid phone number format");

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(1600);
        }
    }
}
