using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Village;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.CreateVillage;

public class CreateVillageCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<CreateVillageCommand, Result<CreateVillageResponse>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<CreateVillageResponse>> Handle(
        CreateVillageCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _application.Cities
            .AsNoTracking()
            .Include(x => x.Government)
            .ThenInclude(x => x.Country)
            .FirstOrDefaultAsync(
                x => x.CityId == request.CityId,
                cancellationToken);

        if (city is null)
        {
            return Result<CreateVillageResponse>
                .Failure(_localizer["CityNotFound"], 404);
        }

        var (nameAr, nameEn) =
            NormalizeHelper.Normalize(
                request.NameAr,
                request.NameEn);

        var exists = await _application.Villages
            .AnyAsync(x =>
                    (nameAr != null && x.VillageNameAr == nameAr)
                    ||
                    (nameEn != null && x.VillageNameEn == nameEn),
                cancellationToken);

        if (exists)
        {
            return Result<CreateVillageResponse>
                .Failure(_localizer["VillageAlreadyExists"], 409);
        }

        var village = new Domain.Entities.Location.Village
        {
            VillageNameAr = nameAr ?? string.Empty,
            VillageNameEn = nameEn ?? string.Empty,
            CityId = city.CityId,
            GovernmentId = city.GovernmentId,
            CountryId = city.Government!.CountryId
        };

        await _application.Villages
            .AddAsync(village, cancellationToken);

        await _application.SaveChangesAsync(cancellationToken);

        var response = await _application.Villages
            .AsNoTracking()
            .Include(x => x.City)
            .ThenInclude(x => x.Government)
            .ThenInclude(x => x.Country)
            .Where(x => x.VillageId == village.VillageId)
            .Select(x => new CreateVillageResponse
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
                }
            })
            .FirstAsync(cancellationToken);

        return Result<CreateVillageResponse>.Success(
            response,
            _localizer["VillageCreatedSuccessfully"]);
    }
}