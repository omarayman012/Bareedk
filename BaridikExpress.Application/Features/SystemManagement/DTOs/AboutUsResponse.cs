using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record AboutUsResponse(
    LocalizedDto? Title,
    LocalizedDto? Description,
    string? ImageUrl,
    string? VideoUrl,
    string? ExternalLinkYoutube,
    string? UpdatedBy,
    DateTime? UpdatedAt
);