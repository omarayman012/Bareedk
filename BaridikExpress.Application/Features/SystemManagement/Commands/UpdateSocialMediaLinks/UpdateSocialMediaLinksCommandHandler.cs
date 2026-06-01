using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSocialMediaLinks;

public sealed class UpdateSocialMediaLinksCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateSocialMediaLinksCommand, Result<List<SocialMediaLinksResponse>>>
{
    #region Handle

    public async Task<Result<List<SocialMediaLinksResponse>>> Handle(
        UpdateSocialMediaLinksCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch All Links

        var allLinks = await db.SocialMediaLinks
            .ToListAsync(cancellationToken);

        #endregion

        #region Update Only Sent Links

        var sentLinks = request.Links
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .ToDictionary(
                x => x.PlatformName.Trim().ToLowerInvariant(),
                x => x.Url.Trim());

        foreach (var link in allLinks)
        {
            var key = link.PlatformName.ToLowerInvariant();
            if (sentLinks.TryGetValue(key, out var newUrl))
                link.Update(url: newUrl);
        }

        #endregion

        #region Save

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        #region Map Response

        var response = allLinks
            .Select(x => new SocialMediaLinksResponse(x.PlatformName, x.Url))
            .ToList();

        #endregion

        return Result<List<SocialMediaLinksResponse>>
            .Success(response, localizer["SocialMediaLinksUpdatedSuccessfully"]);
    }

    #endregion
}