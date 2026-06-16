using BaridikExpress.Application.Common.Abstractions;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Commands.UpdateCurrency;

public record UpdateCurrencyCommand(
    Guid Id,
    string? NameEn,
    string? NameAr,
    string CurrencyCode,
    string? CurrencySymbol,
    bool IsActive
) : IRequest<Result<Guid>>;