using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Commands.ToggleBannerStatus;

public class ToggleBannerStatusCommandHandler(
    IGenericRepository<Banner> repo,
    IStringLocalizer localizer
) : IRequestHandler<
        ToggleBannerStatusCommand,
        Result<bool>
    >
{
    private readonly IGenericRepository<Banner > _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        ToggleBannerStatusCommand request,
        CancellationToken cancellationToken)
    {
        var careerField = await _repo.GetByIdAsync(request.Id);

        if (careerField is null)
            return Result<bool>.Failure(
                _localizer["BannerNotFound"],
                404 );

        careerField.ToggleStatus();
        await _repo.UpdateAsync(careerField);
        return Result<bool>.Success(
           true,
            _localizer["OperationCompletedSuccessfully"]
        );
    }
}