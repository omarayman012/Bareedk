namespace BaridikExpress.Application.Features.Currencies.DTO;

public record CurrencyDto(
    Guid Id,
    string NameEn,
    string NameAr,
    string CurrencyCode,
    string? CurrencySymbol,
    bool IsActive,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy
);