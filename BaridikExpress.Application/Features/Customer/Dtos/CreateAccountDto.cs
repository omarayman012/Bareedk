using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Customer.Dtos;

public record CreateAccountDto(
    string? TaxRegistrationNumber,
    Currency? Currency,
    decimal? OpeningBalance,
    DateOnly? OpeningBalanceDate,
    string? Note
);
