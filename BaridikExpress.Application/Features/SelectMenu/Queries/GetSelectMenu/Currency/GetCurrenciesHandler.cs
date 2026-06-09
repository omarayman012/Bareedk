using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Currency
{
    public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesQuery, Result<List<CurrencySelectMenuDto>>>
    {
        public Task<Result<List<CurrencySelectMenuDto>>> Handle(
            GetCurrenciesQuery request,
            CancellationToken cancellationToken)
        {
            var currencies = Enum.GetValues<Domain.Enum.Currency>()
                .Select(c =>
                {
                    var member = typeof(Domain.Enum.Currency)
                        .GetMember(c.ToString())
                        .First();

                    var displayName = member
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? c.ToString();

                    return new CurrencySelectMenuDto
                    {
                        Id = (int)c, 
                        Name = displayName
                    };
                })
                .ToList();

            return Task.FromResult(Result<List<CurrencySelectMenuDto>>.Success(currencies));
        }
    }
}