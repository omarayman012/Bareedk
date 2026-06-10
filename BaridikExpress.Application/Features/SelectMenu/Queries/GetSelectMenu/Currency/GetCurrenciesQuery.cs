using BaridikExpress.Application.Features.SelectMenu.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Currency
{
    public record GetCurrenciesQuery : IRequest<Result<List<CurrencySelectMenuDto>>>;
}