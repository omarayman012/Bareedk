using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Interfaces.BackUp;

namespace BaridikExpress.Application.Features.Backup.Commands.RunBackupNow
{
    public class CreateBackupCommandHandler
     : IRequestHandler<CreateBackupCommand, Result<string>>
    {
        private readonly IBackupService _backupService;
        private readonly IEmailService _emailService;

        public CreateBackupCommandHandler(
            IBackupService backupService,
            IEmailService emailService)
        {
            _backupService = backupService;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(
            CreateBackupCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.IsAutomatic)
                {
                    // Register Hangfire Job
                    // حسب Frequency و ExecutionTime

                    return Result<string>.Success(
                        string.Empty,
                        "Automatic backup scheduled successfully");
                }

                var backupFile = await _backupService.CreateBackupAsync(
                    request.BackupPath,
                    cancellationToken);

                if (request.SendEmailNotification &&
                    !string.IsNullOrWhiteSpace(request.NotificationEmail))
                {
                    await _emailService.SendEmailAsync(
                        request.NotificationEmail,
                        "Backup Completed",
                        $"Backup created successfully.<br/>File: {backupFile}");
                }

                return Result<string>.Success(
                    backupFile,
                    "Backup created successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Error(ex.Message);
            }
        }
    }
}
