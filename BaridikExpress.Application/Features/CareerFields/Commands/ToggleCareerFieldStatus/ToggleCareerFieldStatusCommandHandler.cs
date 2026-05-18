using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Commands.ToggleCareerFieldStatus;

public class ToggleCareerFieldStatusCommandHandler(
    IGenericRepository<CareerField> repo,
    IStringLocalizer localizer
) : IRequestHandler<
        ToggleCareerFieldStatusCommand,
        Result<bool>
    >
{
    private readonly IGenericRepository<CareerField> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        ToggleCareerFieldStatusCommand request,
        CancellationToken cancellationToken)
    {
        var careerField = await _repo.GetByIdAsync(request.Id);

        if (careerField is null)
            return Result<bool>.Failure(
                _localizer["CareerFieldNotFound"],
                404 );

        careerField.ToggleStatus();
        await _repo.UpdateAsync(careerField);
        return Result<bool>.Success(
           true,
            _localizer["OperationCompletedSuccessfully"]
        );
    }
}