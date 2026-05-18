using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.City;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.City.GetById;

public class GetCityByIdQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetCityByIdQuery, Result<CityDto>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<CityDto>> Handle(
        GetCityByIdQuery request,
        CancellationToken cancellationToken)
    {
        var city = await _applicationDb.Cities
            .AsNoTracking()
            .Include(x => x.Government)
            .ThenInclude(x => x.Country)
            .Where(x => x.CityId == request.Id)
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
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (city is null)
        {
            return Result<CityDto>
                .Failure(_localizer["CityNotFound"], 404);
        }

        return Result<CityDto>.Success(
            city,
            _localizer["CityRetrievedSuccessfully"]);
    }
}