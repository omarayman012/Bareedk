using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetSocialMediaLinks;

public sealed record GetSocialMediaLinksQuery
    : IRequest<Result<List<SocialMediaLinksResponse>>>;