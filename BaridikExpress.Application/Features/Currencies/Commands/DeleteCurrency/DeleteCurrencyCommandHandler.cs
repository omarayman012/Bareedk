using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public DeleteCurrencyCommandHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids is null || request.Ids.Count == 0)
            return Result<bool>.Failure(_localizer["IdsRequired"]);

        var currencies = await _context.Currencies
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(currencies.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(_localizer["SomeCurrenciesNotFound"]);

        _context.Currencies.RemoveRange(currencies);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["CurrenciesDeletedSuccessfully"]);
    }
}