using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;

public class UpdateCountryCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<UpdateCountryCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _application.Countries
            .FirstOrDefaultAsync(x => x.CountryId == request.Id && x.IsActive, cancellationToken);

        if (country is null)
            return Result<bool>.Failure(_localizer["CountryNotFound"], 404);

        var exists = await _application.Countries
            .AnyAsync(x => x.CountryId != request.Id &&
                          (x.CountryNameAr == request.NameAr || x.CountryNameEn == request.NameEn),
                      cancellationToken);

        if (exists)
            return Result<bool>.Failure(_localizer["CountryAlreadyExists"], 409);

        if (!string.IsNullOrWhiteSpace(request.NameAr))
            country.CountryNameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            country.CountryNameEn = request.NameEn;

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["CountryUpdatedSuccessfully"]);
    }
}