using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Commands.DeleteServices;

public sealed class DeleteServicesCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteServicesCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        DeleteServicesCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Services

        var services = await db.Services
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(services.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeServicesNotFound"]);

        #endregion

        #region Delete & Save

        db.Services.RemoveRange(services);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["ServicesDeletedSuccessfully"]);
    }

    #endregion
}