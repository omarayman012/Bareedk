using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public UpdateCurrencyCommandHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (currency is null)
            return Result<Guid>.Failure(_localizer["CurrencyNotFound"], 404);

        currency.Update(
            request.NameEn,
            request.NameAr,
            request.CurrencyCode,
            request.CurrencySymbol,
            request.IsActive
        );
        _context.Currencies.Update(currency);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(currency.Id, _localizer["CurrencyUpdatedSuccessfully"]);
    }
}