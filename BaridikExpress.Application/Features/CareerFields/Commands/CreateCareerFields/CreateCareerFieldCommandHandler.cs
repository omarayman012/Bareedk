using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields
{
    public class CreateCareerFieldCommandHandler(
        IGenericRepository<CareerField> repo,
        IStringLocalizer localizer
    ) : IRequestHandler<CreateCareerFieldCommand, Result<Guid?>>
    {
        private readonly IGenericRepository<CareerField> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<Guid?>> Handle(
            CreateCareerFieldCommand request,
            CancellationToken cancellationToken)
        {
         
            var (nameAr, nameEn) = NormalizeHelper.Normalize(  request.NameAr, request.NameEn);
            var exists = await _repo.AnyAsync(x => x.Name.En == nameEn || x.Name.Ar == nameAr   );

            if (exists)
                return Result<Guid?>.Failure(_localizer["CareerFieldAlreadyExists"] );

            var careerField = new CareerField(nameEn,  nameAr );
           
            await _repo.AddAsync(careerField);
            return Result<Guid?>.Success(
                careerField.Id,
                _localizer["OperationCompletedSuccessfully"]
            );
        }
    }
}