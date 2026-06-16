using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Currencies.DTO;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Queries.GetAllCurrencies;

public record GetAllCurrenciesQuery(
    string? Search,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedList<CurrencyDto>>>;