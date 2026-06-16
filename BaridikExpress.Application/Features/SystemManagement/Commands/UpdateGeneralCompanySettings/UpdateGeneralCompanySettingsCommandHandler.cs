using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateGeneralCompanySettings;

public sealed class UpdateGeneralCompanySettingsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateGeneralCompanySettingsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateGeneralCompanySettingsCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Settings

        var settings = await db.GeneralCompanySettings
            .FirstOrDefaultAsync(cancellationToken);

        if (settings is null)
            return Result<bool>.Failure(localizer["GeneralCompanySettingsNotFound"], 404);

        #endregion

        #region Update & Save

        settings.Update(
            request.WorkingHoursDuration,
            request.NumberOfRejectedShipmentsByDelivery);

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["GeneralCompanySettingsUpdatedSuccessfully"]);
    }
}