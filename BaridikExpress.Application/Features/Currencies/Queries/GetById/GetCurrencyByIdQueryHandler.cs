using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Currencies.DTO;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Queries.GetCurrencyById;

public class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, Result<CurrencyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public GetCurrencyByIdQueryHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<CurrencyDto>> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new CurrencyDto(
                x.Id,
                x.Name.En,
                x.Name.Ar,
                x.CurrencyCode,
                x.CurrencySymbol,
                x.IsActive,
                x.CreatedAt,
                x.CreatedBy != null ? x.CreatedBy.UserName : null,
                x.UpdatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.UserName : null
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (currency is null)
            return Result<CurrencyDto>.Failure(_localizer["CurrencyNotFound"], 404);

        return Result<CurrencyDto>.Success(currency, _localizer["CurrencyRetrievedSuccessfully"]);
    }
}