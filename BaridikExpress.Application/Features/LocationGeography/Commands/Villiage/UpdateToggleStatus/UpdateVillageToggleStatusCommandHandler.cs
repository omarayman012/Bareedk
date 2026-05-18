using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateToggleStatus;

public class UpdateVillageToggleStatusCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateVillageToggleStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateVillageToggleStatusCommand request,
        CancellationToken cancellationToken)
    {
        var village = await _application.Villages
            .FirstOrDefaultAsync(
                x => x.VillageId == request.Id,
                cancellationToken);

        if (village is null)
        {
            return Result<bool>
                .Failure(_localizer["VillageNotFound"], 404);
        }

        village.ToggleStatus();

        await _application.SaveChangesAsync(cancellationToken);

        var message = village.IsActive
            ? _localizer["VillageActivatedSuccessfully"]
            : _localizer["VillageDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}