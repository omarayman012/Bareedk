namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record SystemManagementResponse(
    Guid Id,
    string? DescriptionAr,
    string? DescriptionEn,
    string? UpdatedBy,
    DateTime? UpdatedAt
);