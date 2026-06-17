using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.BackupModules;

public sealed class BackupHistory : BaseEntity
{
    public Guid Id { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public long FileSizeInBytes { get; private set; }
    public BackupStatus Status { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime BackupDate { get; private set; }
    public bool IsManual { get; private set; }

    private BackupHistory() { }

    public static BackupHistory Create(
        string fileName,
        string filePath,
        long fileSizeInBytes,
        BackupStatus status,
        bool isManual = false,
        string? errorMessage = null)
    {
        return new BackupHistory
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            FilePath = filePath,
            FileSizeInBytes = fileSizeInBytes,
            Status = status,
            IsManual = isManual,
            ErrorMessage = errorMessage,
            BackupDate = DateTime.UtcNow,
        };
    }

    public void MarkAsSuccess(string filePath, long fileSizeInBytes)
    {
        FilePath = filePath;
        FileSizeInBytes = fileSizeInBytes;
        Status = BackupStatus.Success;
        ErrorMessage = null;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = BackupStatus.Failed;
        ErrorMessage = errorMessage;
    }
}