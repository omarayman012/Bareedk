// BackupSetting.cs
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.BackupModules;

public sealed class BackupSetting : BaseEntity
{
    public Guid Id { get; private set; }
    public bool IsAutoBackupEnabled { get; private set; }
    public BackupFrequency Frequency { get; private set; }
    public TimeOnly ExecutionTime { get; private set; }
    public string? StoragePath { get; private set; }
    public bool SendEmailNotification { get; private set; }
    public string? NotificationEmail { get; private set; }
    public int RetentionDays { get; private set; } = 30;

    private BackupSetting() { }

    public static BackupSetting Create(
        bool isAutoBackupEnabled,
        BackupFrequency frequency,
        TimeOnly executionTime,
        string? storagePath = null,
        bool sendEmailNotification = false,
        string? notificationEmail = null,
        int retentionDays = 30)
    {
        return new BackupSetting
        {
            Id = Guid.NewGuid(),
            IsAutoBackupEnabled = isAutoBackupEnabled,
            Frequency = frequency,
            ExecutionTime = executionTime,
            StoragePath = storagePath,
            SendEmailNotification = sendEmailNotification,
            NotificationEmail = notificationEmail,
            RetentionDays = retentionDays,
        };
    }

    public void Update(
        bool? isAutoBackupEnabled = null,
        BackupFrequency? frequency = null,
        TimeOnly? executionTime = null,
        string? storagePath = null,
        bool? sendEmailNotification = null,
        string? notificationEmail = null,
        int? retentionDays = null)
    {
        if (isAutoBackupEnabled.HasValue) IsAutoBackupEnabled = isAutoBackupEnabled.Value;
        if (frequency.HasValue) Frequency = frequency.Value;
        if (executionTime.HasValue) ExecutionTime = executionTime.Value;
        if (storagePath is not null) StoragePath = storagePath;
        if (sendEmailNotification.HasValue) SendEmailNotification = sendEmailNotification.Value;
        if (notificationEmail is not null) NotificationEmail = notificationEmail;
        if (retentionDays.HasValue) RetentionDays = retentionDays.Value;
    }
}