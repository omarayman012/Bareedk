using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Backup.Commands.SaveBackupSettings;

public sealed record SaveBackupSettingsCommand(
    bool IsAutoBackupEnabled,
    BackupFrequency Frequency,
    TimeOnly ExecutionTime,
    string? StoragePath,
    bool SendEmailNotification,
    string? NotificationEmail,
    int RetentionDays
) : IRequest<Result<Guid>>;