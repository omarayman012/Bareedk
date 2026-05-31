using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSocialMediaLinks;

public sealed class UpdateSocialMediaLinksCommand
    : IRequest<Result<List<SocialMediaLinksResponse>>>
{
    public List<UpdateSocialMediaLinkDto> Links { get; set; } = [];
}

public sealed record UpdateSocialMediaLinkDto(
    string PlatformName,
    string Url);