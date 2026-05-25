using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.DeleteDeliveryTypes;

public sealed class DeleteDeliveryTypesCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteDeliveryTypesCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        DeleteDeliveryTypesCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch DeliveryTypes

        var deliveryTypes = await db.DeliveryTypes
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(deliveryTypes.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeDeliveryTypesNotFound"]);

        #endregion

        #region Delete & Save

        db.DeliveryTypes.RemoveRange(deliveryTypes);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["DeliveryTypesDeletedSuccessfully"]);
    }

    #endregion
}