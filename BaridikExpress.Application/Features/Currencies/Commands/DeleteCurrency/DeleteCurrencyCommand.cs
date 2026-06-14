using BaridikExpress.Application.Common.Abstractions;
using MediatR;

namespace BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;

public record DeleteCurrencyCommand(Guid Id) : IRequest<Result<Guid>>;