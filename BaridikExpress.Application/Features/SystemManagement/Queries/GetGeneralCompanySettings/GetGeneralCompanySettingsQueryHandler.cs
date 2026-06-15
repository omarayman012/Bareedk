using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetGeneralCompanySettings;

public sealed class GetGeneralCompanySettingsQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetGeneralCompanySettingsQuery, Result<GeneralCompanySettingsResponse>>
{
    public async Task<Result<GeneralCompanySettingsResponse>> Handle(
        GetGeneralCompanySettingsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await db.GeneralCompanySettings
            .AsNoTracking()
            .Select(x => new GeneralCompanySettingsResponse(
                x.Id,
                x.WorkingHoursDuration,
                x.NumberOfRejectedShipmentsByDelivery,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result<GeneralCompanySettingsResponse>
                .Failure(localizer["GeneralCompanySettingsNotFound"], 404);

        return Result<GeneralCompanySettingsResponse>
            .Success(response, localizer["GeneralCompanySettingsRetrievedSuccessfully"]);
    }
}