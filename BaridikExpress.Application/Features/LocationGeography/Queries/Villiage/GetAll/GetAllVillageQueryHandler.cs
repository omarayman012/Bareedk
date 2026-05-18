using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Village;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetAll;

public class GetAllVillageQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllVillageQuery,
        Result<PaginatedList<VillageDto>>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<PaginatedList<VillageDto>>> Handle(
        GetAllVillageQuery request,
        CancellationToken cancellationToken)
    {
        var query = _applicationDb.Villages
            .AsNoTracking()
            .Include(x => x.City)
            .ThenInclude(x => x.Government)
            .ThenInclude(x => x.Country)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim().ToLower();

            query = query.Where(x =>
                x.VillageNameAr.ToLower().Contains(name) ||
                x.VillageNameEn.ToLower().Contains(name));
        }

        if (request.CityId.HasValue)
        {
            query = query.Where(x =>
                x.CityId == request.CityId.Value);
        }

        if (request.GovernmentId.HasValue)
        {
            query = query.Where(x =>
                x.City!.GovernmentId == request.GovernmentId.Value);
        }

        if (request.CountryId.HasValue)
        {
            query = query.Where(x =>
                x.City!.Government!.CountryId ==
                request.CountryId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        var villagesQuery = query.Select(x => new VillageDto
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
        });

        var paginatedVillages =
            await PaginatedList<VillageDto>.CreateAsync(
                villagesQuery,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<VillageDto>>
            .Success(
                paginatedVillages,
                _localizer["VillagesRetrievedSuccessfully"]);
    }
}