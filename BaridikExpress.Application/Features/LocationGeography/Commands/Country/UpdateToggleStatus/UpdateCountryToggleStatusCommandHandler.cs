namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateToggleStatus;

public class UpdateCountryToggleStatusCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<UpdateCountryToggleStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateCountryToggleStatusCommand request,
        CancellationToken cancellationToken)
    {
        var country = await _application.Countries
            .FirstOrDefaultAsync(x => x.CountryId == request.Id, cancellationToken);

        if (country is null)
            return Result<bool>.Failure(_localizer["CountryNotFound"], 404);

        country.ToggleStatus();

        await _application.SaveChangesAsync(cancellationToken);

        var message = country.IsActive
            ? _localizer["CountryActivatedSuccessfully"]
            : _localizer["CountryDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}