using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.ToggleDeliveryTypeStatus;

public sealed class ToggleDeliveryTypeStatusCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<ToggleDeliveryTypeStatusCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        ToggleDeliveryTypeStatusCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch DeliveryType

        var deliveryType = await db.DeliveryTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (deliveryType is null)
            return Result<bool>.Failure(localizer["DeliveryTypeNotFound"], 404);

        #endregion

        #region Toggle & Save

        deliveryType.ToggleStatus();
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        var message = deliveryType.IsSwitchActive
            ? localizer["DeliveryTypeActivatedSuccessfully"]
            : localizer["DeliveryTypeDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }

    #endregion
}