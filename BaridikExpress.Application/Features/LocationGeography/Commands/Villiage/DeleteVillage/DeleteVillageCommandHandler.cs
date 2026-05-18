namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.DeleteVillage;

public class DeleteVillageCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteVillageCommand, Result<bool>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteVillageCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids is null || request.Ids.Count == 0)
        {
            return Result<bool>
                .Failure(_localizer["IdsRequired"], 400);
        }

        var villages = await _application.Villages
            .Where(x => request.Ids.Contains(x.VillageId))
            .ToListAsync(cancellationToken);

        if (villages.Count == 0)
        {
            return Result<bool>
                .Failure(_localizer["VillageNotFound"], 404);
        }

        if (villages.Count != request.Ids.Count)
        {
            var notFoundIds = request.Ids
                .Except(villages.Select(x => x.VillageId))
                .ToList();

            return Result<bool>.Failure(
                $"{_localizer["VillagesNotFound"]}: {string.Join(", ", notFoundIds)}",
                404);
        }

        _application.Villages.RemoveRange(villages);

        await _application.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["DeleteVillagesSuccessfully"]);
    }
}