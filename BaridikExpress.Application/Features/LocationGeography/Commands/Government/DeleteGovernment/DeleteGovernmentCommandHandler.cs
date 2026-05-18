using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.DeleteGovernment;

public class DeleteGovernmentCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteGovernmentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteGovernmentCommand request,
        CancellationToken cancellationToken)
    {
       

        var governments = await _application.Governments
            .Where(x => request.Ids.Contains(x.GovernmentId))
            .ToListAsync(cancellationToken);

        if (governments.Count == 0)
        {
            return Result<bool>
                .Failure(_localizer["GovernmentNotFound"], 404);
        }

        if (governments.Count != request.Ids.Count)
        {
            var notFoundIds = request.Ids
                .Except(governments.Select(x => x.GovernmentId))
                .ToList();

            return Result<bool>.Failure(
                $"{_localizer["GovernmentsNotFound"]}: {string.Join(", ", notFoundIds)}",
                404);
        }

        var hasChildren = await _application.Cities
            .AnyAsync(x =>
                request.Ids.Contains(x.GovernmentId),
                cancellationToken);

        if (hasChildren)
        {
            return Result<bool>
                .Failure(_localizer["GovernmentHasCities"], 409);
        }

        _application.Governments.RemoveRange(governments);

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["DeleteGovernmentsSuccessfully"]);
    }
}