using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.City;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetAll;

public class GetAllCityQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllCityQuery,
        Result<PaginatedList<CityDto>>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<PaginatedList<CityDto>>> Handle(
        GetAllCityQuery request,
        CancellationToken cancellationToken)
    {
        var query = _applicationDb.Cities
            .AsNoTracking()
            .Include(x => x.Government)
            .ThenInclude(x => x.Country)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim().ToLower();

            query = query.Where(x =>
                x.CityNameAr.ToLower().Contains(name) ||
                x.CityNameEn.ToLower().Contains(name));
        }

        if (request.GovernmentId.HasValue)
        {
            query = query.Where(x =>
                x.GovernmentId == request.GovernmentId.Value);
        }

        if (request.CountryId.HasValue)
        {
            query = query.Where(x =>
                x.Government!.CountryId == request.CountryId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        var citiesQuery = query.Select(x => new CityDto
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
                Id = x.Government.Country!.CountryId,
                AR = x.Government.Country.CountryNameAr,
                EN = x.Government.Country.CountryNameEn
            },

            CreatedBy = x.CreatedBy != null
                ? x.CreatedBy.FullName
                : "",

            CreatedAt = x.CreatedAt,

            UpdatedBy = x.UpdatedBy != null
                ? x.UpdatedBy.FullName
                : x.UpdatedById,

            UpdatedAt = x.UpdatedAt,

            IsActive = x.IsActive
        });

        var paginatedCities =
            await PaginatedList<CityDto>.CreateAsync(
                citiesQuery,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<CityDto>>
            .Success(
                paginatedCities,
                _localizer["CitiesRetrievedSuccessfully"]);
    }
}