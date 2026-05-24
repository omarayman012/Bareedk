using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Commands.UpdateCareerFields;

public class UpdateCareerFieldCommandHandler(
    IGenericRepository<CareerField> repo,
    IStringLocalizer localizer
) : IRequestHandler<UpdateCareerFieldCommand, Result<bool>>
{
    private readonly IGenericRepository<CareerField> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateCareerFieldCommand request,
        CancellationToken cancellationToken)
    {
        var careerField = await _repo.GetByIdAsync(request.Id);
        if (careerField is null)
            return Result<bool>.Failure( _localizer["CareerFieldNotFound"] );


        var nameAr = string.IsNullOrWhiteSpace(request.NameAr) ? careerField.Name.Ar : request.NameAr;
        var nameEn = string.IsNullOrWhiteSpace(request.NameEn) ? careerField.Name.En : request.NameEn;

        (nameAr, nameEn) = NormalizeHelper.Normalize(
            nameAr,
            nameEn
        );

        var exists = await _repo.AnyAsync(x =>
            x.Id != request.Id &&
            (
                x.Name.En == nameEn ||
                x.Name.Ar == nameAr
            )
        );

        if (exists)
            return Result<bool>.Failure(_localizer["CareerFieldAlreadyExists"]);

        careerField.Update(nameEn, nameAr);
        await _repo.UpdateAsync(careerField);

        return Result<bool>.Success(
            true,
            _localizer["OperationCompletedSuccessfully"]);
    }
}