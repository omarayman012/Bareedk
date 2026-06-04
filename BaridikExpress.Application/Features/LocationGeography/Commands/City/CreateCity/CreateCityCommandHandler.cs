using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.City;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.CreateCity;

public class CreateCityCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<CreateCityCommand, Result<CityDto>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<CityDto>> Handle(
        CreateCityCommand request,
        CancellationToken cancellationToken)
    {
        #region Check Government
        var government = await _application.Governments
            .AsNoTracking()
            .Include(x => x.Country)
            .FirstOrDefaultAsync(
                x => x.GovernmentId == request.GovernmentId && x.IsActive,
                cancellationToken);

        if (government is null)
            return Result<CityDto>.Failure(_localizer["GovernmentNotFound"], 404);
        #endregion

        #region Check Country
        var countryExists = await _application.Countries
            .AnyAsync(
                x => x.CountryId == request.CountryId && x.IsActive,
                cancellationToken);

        if (!countryExists)
            return Result<CityDto>.Failure(_localizer["CountryNotFound"], 404);
        #endregion

        #region Check Duplicate
        var (nameAr, nameEn) = NormalizeHelper.Normalize(request.NameAr, request.NameEn);

        var exists = await _application.Cities
            .AnyAsync(x =>
                (nameAr != null && x.CityNameAr == nameAr) ||
                (nameEn != null && x.CityNameEn == nameEn),
                cancellationToken);

        if (exists)
            return Result<CityDto>.Failure(_localizer["CityAlreadyExists"], 409);
        #endregion

        #region Create
        var city = new Domain.Entities.Location.City
        {
            CityNameAr = nameAr,
            CityNameEn = nameEn,
            GovernmentId = request.GovernmentId,
            CountryId = request.CountryId
        };

        await _application.Cities.AddAsync(city, cancellationToken);
        await _application.SaveChangesAsync(cancellationToken);
        #endregion

        #region Map Response
        var response = await _application.Cities
            .AsNoTracking()
            .Include(x => x.Government)
            .ThenInclude(x => x.Country)
            .Where(x => x.CityId == city.CityId)
            .Select(x => new CityDto
            {
                Id = x.CityId,
                Name = new LocalizedDto
                {
                    AR = x.CityNameAr,
                    EN = x.CityNameEn
                },
                GovernmentName = new LocalizedNameDto
                {
                    Id = x.Government!.GovernmentId,
                    AR = x.Government.GovernmentNameAr,
                    EN = x.Government.GovernmentNameEn
                },
                CountryName = new LocalizedNameDto
                {
                    Id = x.Government!.Country!.CountryId,
                    AR = x.Government.Country.CountryNameAr,
                    EN = x.Government.Country.CountryNameEn
                },
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : "",
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : x.UpdatedById,
                UpdatedAt = x.UpdatedAt,
                IsActive = x.IsActive
            })
            .FirstAsync(cancellationToken);
        #endregion

        return Result<CityDto>.Success(response, _localizer["CityCreatedSuccessfully"]);
    }
}