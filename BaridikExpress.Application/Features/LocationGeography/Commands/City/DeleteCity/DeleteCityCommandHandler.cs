using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.DeleteCity;

public class DeleteCityCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<DeleteCityCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteCityCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids is null || request.Ids.Count == 0)
            return Result<bool>.Failure(_localizer["IdsRequired"], 400);

        var cities = await _application.Cities
            .Where(x => request.Ids.Contains(x.CityId))
            .ToListAsync(cancellationToken);

        if (cities.Count == 0)
            return Result<bool>.Failure(_localizer["CityNotFound"], 404);

        if (cities.Count != request.Ids.Count)
        {
            var notFoundIds = request.Ids
                .Except(cities.Select(x => x.CityId))
                .ToList();

            return Result<bool>.Failure(
                $"{_localizer["CitiesNotFound"]}: {string.Join(", ", notFoundIds)}", 404);
        }

        var hasChildren = await _application.Villages
            .AnyAsync(x => request.Ids.Contains(x.CityId), cancellationToken);

        if (hasChildren)
            return Result<bool>.Failure(_localizer["CityHasVillages"], 409);

        _application.Cities.RemoveRange(cities);
        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["DeleteCitiesSuccessfully"]);
    }
}