using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Currencies.DTO;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Queries.GetCurrencyById;

public record GetCurrencyByIdQuery(Guid Id) : IRequest<Result<CurrencyDto>>;