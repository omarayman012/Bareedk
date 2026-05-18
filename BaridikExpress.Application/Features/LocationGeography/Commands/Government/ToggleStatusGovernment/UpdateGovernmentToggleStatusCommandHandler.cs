using BaridikExpress.Application.Features.LocationGeography.Commands.Government.ToggleStatusGovernment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateToggleStatus;

public class UpdateGovernmentToggleStatusCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateGovernmentToggleStatusCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateGovernmentToggleStatusCommand request,
        CancellationToken cancellationToken)
    {
        var government = await _application.Governments
            .FirstOrDefaultAsync(
                x => x.GovernmentId == request.Id,
                cancellationToken);

        if (government is null)
        {
            return Result<bool>
                .Failure(_localizer["GovernmentNotFound"], 404);
        }

        government.ToggleStatus();

        await _application.SaveChangesAsync(cancellationToken);

        var message = government.IsActive
            ? _localizer["GovernmentActivatedSuccessfully"]
            : _localizer["GovernmentDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}