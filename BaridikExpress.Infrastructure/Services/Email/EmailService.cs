using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.AuthModules;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BaridikExpress.Infrastructure.Services.Email;

public class EmailService : IEmailSender, IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IOptions<MailSettings> mailSettings,
        ILogger<EmailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        string email,
        string subject,
        string htmlMessage)
    {
        using var smtp = new SmtpClient();

        try
        {
            smtp.Timeout = 10000;

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _mailSettings.DisplayName,
                    _mailSettings.UserName));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            await smtp.ConnectAsync(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _mailSettings.UserName,
                _mailSettings.Password);

            await smtp.SendAsync(message);

            _logger.LogInformation(
                "Email sent successfully to {Email}",
                email);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email to {Email}",
                email);

            throw;
        }
        finally
        {
            if (smtp.IsConnected)
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
    public async Task SendEmailWithAttachmentAsync(
    string email,
    string subject,
    string htmlMessage,
    IFormFile file)
    {
        using var smtp = new SmtpClient();
        try
        {
            smtp.Timeout = 10000;

            var message = new MimeMessage();
            message.From.Add(
                new MailboxAddress(
                    _mailSettings.DisplayName,
                    _mailSettings.UserName));
            message.To.Add(
                MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            builder.Attachments.Add(
                file.FileName,
                file.OpenReadStream(),
                ContentType.Parse(file.ContentType));
            message.Body = builder.ToMessageBody();

            await smtp.ConnectAsync(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _mailSettings.UserName,
                _mailSettings.Password);
            await smtp.SendAsync(message);

            _logger.LogInformation(
                "Email with attachment sent successfully to {Email}",
                email);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email with attachment to {Email}",
                email);
            throw;
        }
        finally
        {
            if (smtp.IsConnected)
                await smtp.DisconnectAsync(true);
        }
    }
    public async Task SendResetPasswordEmail(
        User user,
        string otp)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "ForgetPassword",
            new Dictionary<string, string>
            {
                { "{{name}}", user.FullName },
                { "{{OTP}}", otp }
            });

        await SendEmailAsync(
            user.Email!,
            "BaridikExpress: Change Password",
            emailBody);
    }

    public async Task SendConfirmationEmail(
        User user,
        string otp)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody(
            "EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}", user.FullName },
                { "{{OTP}}", otp }
            });

        await SendEmailAsync(
            user.Email!,
            "BaridikExpress: Email Confirmation",
            emailBody);
    }
}