using BaridikExpress.Application.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record SystemManagementResponse(
    Guid Id,
    LocalizeLang? Description,
    string? UpdatedBy,
    DateTime? UpdatedAt
);