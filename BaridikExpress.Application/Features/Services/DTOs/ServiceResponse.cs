using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Services.DTOs;

public sealed record ServiceResponse(
    Guid Id,
    LocalizedDto Name,
    decimal Price,
    Currency Currency,
    string? Image,
    bool IsActive,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
);