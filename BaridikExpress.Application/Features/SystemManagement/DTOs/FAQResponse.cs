using BaridikExpress.Application.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record FAQResponse(
    Guid Id,
    LocalizeLang Question,
    LocalizeLang Answer,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);