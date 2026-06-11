using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.BackUp
{
    public interface IBackupService
    {
        Task<string> CreateBackupAsync(
            string backupPath,
            CancellationToken cancellationToken = default);
    }
}
