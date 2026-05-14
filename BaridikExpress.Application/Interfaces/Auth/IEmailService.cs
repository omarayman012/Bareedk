using BaridikExpress.Domain.Entities.AuthModules;

namespace BaridikExpress.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendResetPasswordEmail(User user, string OTP);
        Task SendConfirmationEmail(User user, string OTP);
    }
}
