using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetServiceByIdQuery, Result<ServiceResponse>>
{
    #region Handle

    public async Task<Result<ServiceResponse>> Handle(
        GetServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        #region Query & Map

        var response = await db.Services
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new ServiceResponse(
                x.Id,
                new LocalizedDto { EN = x.NameEn, AR = x.NameAr },
                x.Price,
                x.ImageUrl,
                x.IsActive,
                x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        #endregion

        if (response is null)
            return Result<ServiceResponse>.Failure(localizer["ServiceNotFound"], 404);

        return Result<ServiceResponse>.Success(response, localizer["ServiceRetrievedSuccessfully"]);
    }

    #endregion
}