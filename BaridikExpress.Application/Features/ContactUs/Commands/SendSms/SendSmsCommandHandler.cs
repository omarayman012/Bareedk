using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Interfaces.Auth;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendSms
{
    public sealed class SendSmsCommandHandler(
     ISmsService smsService,
     IStringLocalizer localizer)
     : IRequestHandler<SendSmsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            SendSmsCommand request,
            CancellationToken cancellationToken)
        {
            await smsService.SendSmsAsync(
                request.PhoneNumber,
                request.Message);

            return Result<bool>.Success(true, localizer["SmsSentSuccessfully"]);
        }
    }
}
