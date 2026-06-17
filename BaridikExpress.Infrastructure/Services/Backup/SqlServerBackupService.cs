using BaridikExpress.Application.Interfaces.BackUp;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Infrastructure.Services.Backup;

public sealed class SqlServerBackupService : IBackupService
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
        if (string.IsNullOrWhiteSpace(backupPath))
            throw new ArgumentException("Backup path is required.", nameof(backupPath));

        if (!Directory.Exists(backupPath))
            Directory.CreateDirectory(backupPath);

        var databaseName = _context.Database.GetDbConnection().Database;

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new InvalidOperationException("Database name could not be resolved.");

        var fileName = $"{databaseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.bak";
        var fullPath = Path.Combine(backupPath, fileName);

        var safeDatabaseName = databaseName.Replace("]", "]]");
        var safeFullPath = fullPath.Replace("'", "''");

        var sql = $"""
                   BACKUP DATABASE [{safeDatabaseName}]
                   TO DISK = N'{safeFullPath}'
                   WITH INIT, STATS = 10
                   """;

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                sql,
                cancellationToken);

            return fullPath;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"SQL Server backup failed. Database: {databaseName}, Path: {fullPath}. Error: {ex.Message}",
                ex);
        }
    }
}