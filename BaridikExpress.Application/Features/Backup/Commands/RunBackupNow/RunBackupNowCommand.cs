namespace BaridikExpress.Application.Features.Backup.Commands.RunBackupNow;

public sealed record RunBackupNowCommand(
    string BackupPath,
    bool SendEmailNotification,
    string? NotificationEmail
) : IRequest<Result<string>>;