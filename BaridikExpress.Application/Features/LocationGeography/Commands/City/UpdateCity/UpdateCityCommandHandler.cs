using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateCity;

public class UpdateCityCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<UpdateCityCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateCityCommand request,
        CancellationToken cancellationToken)
    {
        #region Check City
        var city = await _application.Cities
            .FirstOrDefaultAsync(x => x.CityId == request.Id, cancellationToken);

        if (city is null)
            return Result<bool>.Failure(_localizer["CityNotFound"], 404);
        #endregion

        #region Check Government
        if (request.GovernmentId != Guid.Empty && request.GovernmentId != city.GovernmentId)
        {
            var governmentExists = await _application.Governments
                .AnyAsync(x => x.GovernmentId == request.GovernmentId && x.IsActive, cancellationToken);

            if (!governmentExists)
                return Result<bool>.Failure(_localizer["GovernmentNotFound"], 404);

            city.GovernmentId = request.GovernmentId;
        }
        #endregion

        #region Check Country
        if (request.CountryId.HasValue && request.CountryId != city.CountryId)
        {
            var countryExists = await _application.Countries
                .AnyAsync(x => x.CountryId == request.CountryId.Value && x.IsActive, cancellationToken);

            if (!countryExists)
                return Result<bool>.Failure(_localizer["CountryNotFound"], 404);

            city.CountryId = request.CountryId.Value;
        }
        #endregion

        #region Check Duplicate
        if (!string.IsNullOrWhiteSpace(request.NameAr) || !string.IsNullOrWhiteSpace(request.NameEn))
        {
            var exists = await _application.Cities
                .AnyAsync(x => x.CityId != request.Id &&
                               x.GovernmentId == city.GovernmentId &&
                              (x.CityNameAr == request.NameAr || x.CityNameEn == request.NameEn),
                          cancellationToken);

            if (exists)
                return Result<bool>.Failure(_localizer["CityAlreadyExists"], 409);

            if (!string.IsNullOrWhiteSpace(request.NameAr))
                city.CityNameAr = request.NameAr;

            if (!string.IsNullOrWhiteSpace(request.NameEn))
                city.CityNameEn = request.NameEn;
        }
        #endregion

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["CityUpdatedSuccessfully"]);
    }
}