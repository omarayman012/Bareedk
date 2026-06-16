using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Interfaces.BackUp;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Infrastructure.Services.Backup
{
    public class SqlServerBackupService : IBackupService
    {
        private readonly ApplicationDbContext _context;

        public SqlServerBackupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateBackupAsync(
            string backupPath,
            CancellationToken cancellationToken = default)
        {
            var databaseName = _context.Database.GetDbConnection().Database;

            var fileName =
                $"{databaseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.bak";

            var fullPath = Path.Combine(backupPath, fileName);

            var sql =
                $"BACKUP DATABASE [{databaseName}] TO DISK = N'{fullPath}' WITH INIT";

            await _context.Database.ExecuteSqlRawAsync(
                sql,
                cancellationToken);

            return fullPath;
        }
    }
}
