using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.DeleteCountry;

public class DeleteCountryCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<DeleteCountryCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteCountryCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids is null || request.Ids.Count == 0)
            return Result<bool>.Failure(_localizer["IdsRequired"], 400);

        var countries = await _application.Countries
            .Where(x => request.Ids.Contains(x.CountryId))
            .ToListAsync(cancellationToken);

        if (countries.Count == 0)
            return Result<bool>.Failure(_localizer["CountryNotFound"], 404);

        if (countries.Count != request.Ids.Count)
        {
            var notFoundIds = request.Ids
                .Except(countries.Select(x => x.CountryId))
                .ToList();

            return Result<bool>.Failure(
                $"{_localizer["CountriesNotFound"]}: {string.Join(", ", notFoundIds)}", 404);
        }

        var hasChildren = await _application.Governments
            .AnyAsync(x => request.Ids.Contains(x.CountryId), cancellationToken);

        if (hasChildren)
            return Result<bool>.Failure(_localizer["CountryHasGovernments"], 409);

        _application.Countries.RemoveRange(countries);
        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["DeleteCountriesSuccessfully"]);
    }
}