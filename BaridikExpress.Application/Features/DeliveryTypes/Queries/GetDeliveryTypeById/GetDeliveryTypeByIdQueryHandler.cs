using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.DeliveryTypes.DTO;
using Microsoft.EntityFrameworkCore;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.DeliveryTypes.Queries.GetDeliveryTypeById;

public sealed class GetDeliveryTypeByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetDeliveryTypeByIdQuery, Result<DeliveryTypeResponse>>
{
    #region Handle

    public async Task<Result<DeliveryTypeResponse>> Handle(
        GetDeliveryTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        #region Query & Map

        var response = await db.DeliveryTypes
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new DeliveryTypeResponse(
                x.Id,
                new LocalizedDto { EN = x.NameEn, AR = x.NameAr },
                x.DaysFrom,
                x.DaysTo,
                x.PricePerShipment,
                x.DaysTo * x.PricePerShipment,
                x.Currency,
                x.IsSwitchActive,
                x.ImageUrl,
                new LocalizedDto { EN = x.DescriptionEn, AR = x.DescriptionAr },
                x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        #endregion

        if (response is null)
            return Result<DeliveryTypeResponse>.Failure(localizer["DeliveryTypeNotFound"], 404);

        return Result<DeliveryTypeResponse>
            .Success(response, localizer["DeliveryTypeRetrievedSuccessfully"]);
    }

    #endregion
}