using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateVillage;

public class UpdateVillageCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateVillageCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateVillageCommand request,
        CancellationToken cancellationToken)
    {
        var village = await _application.Villages
            .FirstOrDefaultAsync(
                x => x.VillageId == request.Id && x.IsActive,
                cancellationToken);

        if (village is null)
        {
            return Result<bool>
                .Failure(_localizer["VillageNotFound"], 404);
        }

        var nameAr = request.NameAr?.Trim();
        var nameEn = request.NameEn?.Trim();

        var exists = await _application.Villages
            .AnyAsync(x =>
                    x.VillageId != request.Id &&
                    (
                        (!string.IsNullOrWhiteSpace(nameAr) &&
                         x.VillageNameAr == nameAr)
                        ||
                        (!string.IsNullOrWhiteSpace(nameEn) &&
                         x.VillageNameEn == nameEn)
                    ),
                cancellationToken);

        if (exists)
        {
            return Result<bool>
                .Failure(_localizer["VillageAlreadyExists"], 409);
        }

        if (!string.IsNullOrWhiteSpace(nameAr))
        {
            village.VillageNameAr = nameAr;
        }

        if (!string.IsNullOrWhiteSpace(nameEn))
        {
            village.VillageNameEn = nameEn;
        }

        if (request.CityId.HasValue)
        {
            var cityExists = await _application.Cities
                .AnyAsync(
                    x => x.CityId == request.CityId.Value
                         && x.IsActive,
                    cancellationToken);

            if (!cityExists)
            {
                return Result<bool>
                    .Failure(_localizer["CityNotFound"], 404);
            }

            village.CityId = request.CityId.Value;
        }

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["VillageUpdatedSuccessfully"]);
    }
}