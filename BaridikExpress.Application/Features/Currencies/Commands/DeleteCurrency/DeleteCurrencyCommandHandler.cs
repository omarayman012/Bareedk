using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public DeleteCurrencyCommandHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<Guid>> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (currency is null)
            return Result<Guid>.Failure(_localizer["CurrencyNotFound"], 404);

        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(currency.Id, _localizer["CurrencyDeletedSuccessfully"]);
    }
}