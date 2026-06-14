using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.CurrencyModule;

using MediatR;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public CreateCurrencyCommandHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = new Currency(
            request.NameEn,
            request.NameAr,
            request.CurrencyCode,
            request.CurrencySymbol
        );

        await _context.Currencies.AddAsync(currency, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(currency.Id, _localizer["CurrencyCreatedSuccessfully"]);
    }
}