using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Backup.Commands.RunBackupNow
{
    public sealed record CreateBackupCommand(
     string BackupPath,
     bool IsAutomatic,
     bool SendEmailNotification,
     string? NotificationEmail,
     BackupFrequency? Frequency,
     TimeOnly? ExecutionTime)
     : IRequest<Result<string>>;
}
