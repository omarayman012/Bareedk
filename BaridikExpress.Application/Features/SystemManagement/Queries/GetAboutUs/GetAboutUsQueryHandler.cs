using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetAboutUs;

public sealed class GetAboutUsQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAboutUsQuery, Result<AboutUsResponse>>
{
    #region Handle

    public async Task<Result<AboutUsResponse>> Handle(
        GetAboutUsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await db.AboutUs
            .AsNoTracking()
            .Select(x => new AboutUsResponse(
                new LocalizedDto { EN = x.TitleEn, AR = x.TitleAr },
                new LocalizedDto { EN = x.DescriptionEn, AR = x.DescriptionAr },
                x.ImageUrl,
                x.VideoUrl,
                x.ExternalLinkYoutube,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result<AboutUsResponse>.Failure(localizer["AboutUsNotFound"], 404);

        return Result<AboutUsResponse>
            .Success(response, localizer["AboutUsRetrievedSuccessfully"]);
    }

    #endregion
}