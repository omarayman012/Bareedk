using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateToggleStatus;

public class UpdateToggleStatusCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<UpdateCityToggleStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateCityToggleStatusCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _application.Cities
            .FirstOrDefaultAsync(x => x.CityId == request.Id, cancellationToken);

        if (city is null)
            return Result<bool>.Failure(_localizer["CityNotFound"], 404);

        city.ToggleStatus();

        await _application.SaveChangesAsync(cancellationToken);

        var message = city.IsActive
            ? _localizer["CityActivatedSuccessfully"]
            : _localizer["CityDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}