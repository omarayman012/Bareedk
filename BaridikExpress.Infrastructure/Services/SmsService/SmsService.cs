using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BaridikExpress.Infrastructure.Services.SmsService
{
    public class SmsService : ISmsService
    {
        private readonly TwilioSettings _settings;
        private readonly ILogger<SmsService> _logger;

        public SmsService(
            IOptions<TwilioSettings> settings,
            ILogger<SmsService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendSmsAsync(
            string phoneNumber,
            string message)
        {
            try
            {
                TwilioClient.Init(
                    _settings.AccountSid,
                    _settings.AuthToken);

                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(_settings.FromPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                _logger.LogInformation(
                    "SMS sent successfully: {sid}",
                    result.Sid);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "SMS sending failed");

                throw;
            }
        }
    }
}