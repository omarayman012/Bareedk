using BaridikExpress.Domain.Entities.BackupModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Backup.Commands.SaveBackupSettings;

public sealed class SaveBackupSettingsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<SaveBackupSettingsCommand, Result<Guid>>
{
    #region Handle
    public async Task<Result<Guid>> Handle(
        SaveBackupSettingsCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate Auto Backup Data

        if (request.IsAutoBackupEnabled)
        {
            if (string.IsNullOrWhiteSpace(request.StoragePath))
                return Result<Guid>.Failure(localizer["BackupStoragePathRequired"], 400);

            if (request.SendEmailNotification &&
                string.IsNullOrWhiteSpace(request.NotificationEmail))
                return Result<Guid>.Failure(localizer["BackupNotificationEmailRequired"], 400);

            if (request.RetentionDays <= 0)
                return Result<Guid>.Failure(localizer["BackupRetentionDaysMustBeGreaterThanZero"], 400);
        }

        #endregion

        #region Get Existing Settings

        var existingSetting = await db.BackupSettings
            .FirstOrDefaultAsync(cancellationToken);

        #endregion

        #region Create Settings

        if (existingSetting is null)
        {
            var setting = BackupSetting.Create(
                isAutoBackupEnabled: request.IsAutoBackupEnabled,
                frequency: request.Frequency,
                executionTime: request.ExecutionTime,
                storagePath: request.StoragePath,
                sendEmailNotification: request.SendEmailNotification,
                notificationEmail: request.NotificationEmail,
                retentionDays: request.RetentionDays
            );

            await db.BackupSettings.AddAsync(setting, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(
                setting.Id,
                localizer["BackupSettingsSavedSuccessfully"],
                201);
        }

        #endregion

        #region Update Settings

        existingSetting.Update(
            isAutoBackupEnabled: request.IsAutoBackupEnabled,
            frequency: request.Frequency,
            executionTime: request.ExecutionTime,
            storagePath: request.StoragePath,
            sendEmailNotification: request.SendEmailNotification,
            notificationEmail: request.NotificationEmail,
            retentionDays: request.RetentionDays
        );

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<Guid>.Success(
            existingSetting.Id,
            localizer["BackupSettingsUpdatedSuccessfully"]);
    }
    #endregion
}