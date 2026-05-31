using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetSocialMediaLinks;

public sealed class GetSocialMediaLinksQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetSocialMediaLinksQuery, Result<List<SocialMediaLinksResponse>>>
{
    #region Handle

    public async Task<Result<List<SocialMediaLinksResponse>>> Handle(
        GetSocialMediaLinksQuery request,
        CancellationToken cancellationToken)
    {
        var response = await db.SocialMediaLinks
            .AsNoTracking()
            .OrderBy(x => x.PlatformName)
            .Select(x => new SocialMediaLinksResponse(x.PlatformName, x.Url))
            .ToListAsync(cancellationToken);

        return Result<List<SocialMediaLinksResponse>>
            .Success(response, localizer["SocialMediaLinksRetrievedSuccessfully"]);
    }

    #endregion
}