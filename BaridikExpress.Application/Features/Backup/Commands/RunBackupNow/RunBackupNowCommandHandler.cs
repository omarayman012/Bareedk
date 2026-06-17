using BaridikExpress.Application.Interfaces.BackUp;
using BaridikExpress.Domain.Entities.BackupModules;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Backup.Commands.RunBackupNow;

public sealed class RunBackupNowCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IBackupService backupService,
    IEmailService emailService)
    : IRequestHandler<RunBackupNowCommand, Result<string>>
{
    #region Handle
    public async Task<Result<string>> Handle(
        RunBackupNowCommand request,
        CancellationToken cancellationToken)
    {
        #region Validate

        if (string.IsNullOrWhiteSpace(request.BackupPath))
            return Result<string>.Failure(localizer["BackupPathRequired"], 400);

        if (request.SendEmailNotification &&
            string.IsNullOrWhiteSpace(request.NotificationEmail))
            return Result<string>.Failure(localizer["BackupNotificationEmailRequired"], 400);

        #endregion

        BackupHistory? history = null;

        try
        {
            #region Create History In Progress

            history = BackupHistory.Create(
                fileName: string.Empty,
                filePath: request.BackupPath,
                fileSizeInBytes: 0,
                status: BackupStatus.InProgress,
                isManual: true
            );

            await db.BackupHistories.AddAsync(history, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            #endregion

            #region Create Backup

            var backupFilePath = await backupService.CreateBackupAsync(
                request.BackupPath,
                cancellationToken);

            var fileInfo = new FileInfo(backupFilePath);

            #endregion

            #region Update History Success

            history.MarkAsSuccess(
                filePath: backupFilePath,
                fileSizeInBytes: fileInfo.Exists ? fileInfo.Length : 0
            );

            await db.SaveChangesAsync(cancellationToken);

            #endregion

            #region Send Email Notification

            if (request.SendEmailNotification &&
                !string.IsNullOrWhiteSpace(request.NotificationEmail))
            {
                await emailService.SendEmailAsync(
                    request.NotificationEmail,
                    localizer["BackupCompletedEmailSubject"],
                    $"{localizer["BackupCreatedSuccessfully"]}<br/>File: {backupFilePath}");
            }

            #endregion

            return Result<string>.Success(
                backupFilePath,
                localizer["BackupCreatedSuccessfully"],
                201);
        }
        catch (Exception ex)
        {
            #region Update History Failed

            if (history is not null)
            {
                history.MarkAsFailed(ex.Message);
                await db.SaveChangesAsync(cancellationToken);
            }

            #endregion

            return Result<string>.Failure(localizer["BackupFailed"], 500);
        }
    }
    #endregion
}