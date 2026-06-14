using BaridikExpress.Application.Common.Abstractions;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;

public record CreateCurrencyCommand(
    string? NameEn,
    string? NameAr,
    string CurrencyCode,
    string? CurrencySymbol
) : IRequest<Result<Guid>>;