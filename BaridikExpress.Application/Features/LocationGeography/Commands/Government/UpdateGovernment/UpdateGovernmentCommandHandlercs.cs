using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateGovernment;

public class UpdateGovernmentCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateGovernmentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateGovernmentCommand request,
        CancellationToken cancellationToken)
    {
        var government = await _application.Governments
            .FirstOrDefaultAsync(
                x => x.GovernmentId == request.Id && x.IsActive,
                cancellationToken);

        if (government is null)
        {
            return Result<bool>
                .Failure(_localizer["GovernmentNotFound"], 404);
        }

        var exists = await _application.Governments
            .AnyAsync(x =>
                    x.GovernmentId != request.Id &&
                    (
                        (!string.IsNullOrWhiteSpace(request.NameAr) &&
                         x.GovernmentNameAr == request.NameAr)
                        ||
                        (!string.IsNullOrWhiteSpace(request.NameEn) &&
                         x.GovernmentNameEn == request.NameEn)
                    ),
                cancellationToken);

        if (exists)
        {
            return Result<bool>
                .Failure(_localizer["GovernmentAlreadyExists"], 409);
        }

        if (!string.IsNullOrWhiteSpace(request.NameAr))
        {
            government.GovernmentNameAr = request.NameAr;
        }

        if (!string.IsNullOrWhiteSpace(request.NameEn))
        {
            government.GovernmentNameEn = request.NameEn;
        }

        if (request.CountryId.HasValue)
        {
            var countryExists = await _application.Countries
                .AnyAsync(
                    x => x.CountryId == request.CountryId.Value && x.IsActive,
                    cancellationToken);

            if (!countryExists)
            {
                return Result<bool>
                    .Failure(_localizer["CountryNotFound"], 404);
            }

            government.CountryId = request.CountryId.Value;
        }

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["GovernmentUpdatedSuccessfully"]);
    }
}