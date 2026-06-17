using BaridikExpress.Application.Interfaces.Auth;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendSms
{
    public sealed class SendSmsCommandHandler(
        ISmsService smsService,
        IStringLocalizer<SendSmsCommandHandler> localizer)
        : IRequestHandler<SendSmsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            SendSmsCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await smsService.SendSmsAsync(
                    request.PhoneNumber,
                    request.Message);

                return Result<bool>.Success(
                    true,
                    localizer["SmsSentSuccessfully"]);
            }
            catch
            {
                return Result<bool>.Failure(
                    localizer["SmsSendingFailed"]);
            }
        }
    }
}