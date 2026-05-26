using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Commands.ToggleServiceStatus;

public sealed class ToggleServiceStatusCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<ToggleServiceStatusCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        ToggleServiceStatusCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Service

        var service = await db.Services
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (service is null)
            return Result<bool>.Failure(localizer["ServiceNotFound"], 404);

        #endregion

        #region Toggle & Save

        service.ToggleStatus();
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        var message = service.IsActive
            ? localizer["ServiceActivatedSuccessfully"]
            : localizer["ServiceDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }

    #endregion
}