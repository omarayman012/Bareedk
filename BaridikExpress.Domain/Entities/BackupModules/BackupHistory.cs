using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.BackupModules;

public class BackupHistory : BaseEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;

    public string FilePath { get; set; } = default!;

    public long FileSizeInBytes { get; set; }

    public BackupStatus Status { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime BackupDate { get; set; }
}