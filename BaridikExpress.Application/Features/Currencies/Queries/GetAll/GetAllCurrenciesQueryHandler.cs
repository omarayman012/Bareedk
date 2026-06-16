using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Currencies.DTO;
using BaridikExpress.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Queries.GetAllCurrencies;

public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, Result<PaginatedList<CurrencyDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;

    public GetAllCurrenciesQueryHandler(IApplicationDbContext context, IStringLocalizer localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<Result<PaginatedList<CurrencyDto>>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Currencies
      .AsNoTracking()
      .Include(x => x.CreatedBy)
      .Include(x => x.UpdatedBy)
      .AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x =>
                x.Name.En.Contains(request.Search) ||
                x.Name.Ar.Contains(request.Search) ||
                x.CurrencyCode.Contains(request.Search));

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
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
));

        var result = await PaginatedList<CurrencyDto>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        return Result<PaginatedList<CurrencyDto>>
            .Success(result, _localizer["CurrenciesRetrievedSuccessfully"]);
    }
}