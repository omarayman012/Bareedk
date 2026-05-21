using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Village;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetById;

public class GetVillageByIdQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetVillageByIdQuery, Result<VillageDto>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<VillageDto>> Handle(
        GetVillageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var village = await _applicationDb.Villages
            .AsNoTracking()
            .Include(x => x.City)
            .ThenInclude(x => x.Government)
            .ThenInclude(x => x.Country)
            .Where(x => x.VillageId == request.Id)
            .Select(x => new VillageDto
            {
                Id = x.VillageId,

                Name = new LocalizedDto
                {
                    AR = x.VillageNameAr,
                    EN = x.VillageNameEn
                },

                CityName = new LocalizedNameDto
                {
                    Id = x.City!.CityId,
                    AR = x.City.CityNameAr,
                    EN = x.City.CityNameEn
                },

                GovernmentName = new LocalizedNameDto
                {
                    Id = x.City.Government!.GovernmentId,
                    AR = x.City.Government.GovernmentNameAr,
                    EN = x.City.Government.GovernmentNameEn
                },

                CountryName = new LocalizedNameDto
                {
                    Id = x.City.Government.Country!.CountryId,
                    AR = x.City.Government.Country.CountryNameAr,
                    EN = x.City.Government.Country.CountryNameEn
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

        if (village is null)
        {
            return Result<VillageDto>
                .Failure(_localizer["VillageNotFound"], 404);
        }

        return Result<VillageDto>.Success(
            village,
            _localizer["VillageRetrievedSuccessfully"]);
    }
}