public record CurrencyDto(
    Guid Id,
    string NameEn,
    string NameAr,
    string CurrencyCode,
    string? CurrencySymbol,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt
);