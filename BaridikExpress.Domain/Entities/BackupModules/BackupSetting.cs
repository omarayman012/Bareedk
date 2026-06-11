using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.BackupModules;

public class BackupSetting : BaseEntity
{
    public Guid Id { get; set; }

    public string StoragePath { get; set; } = default!;

    public BackupFrequency Frequency { get; set; }

    public TimeOnly ExecutionTime { get; set; }

    public bool SendEmailNotification { get; set; }

    public string? NotificationEmail { get; set; }

    public int RetentionDays { get; set; } = 30;
}