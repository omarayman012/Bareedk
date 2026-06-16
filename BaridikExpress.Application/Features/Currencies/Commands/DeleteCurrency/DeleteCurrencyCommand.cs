using BaridikExpress.Application.Common.Abstractions;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;

public record DeleteCurrencyCommand(List<Guid> Ids) : IRequest<Result<bool>>;