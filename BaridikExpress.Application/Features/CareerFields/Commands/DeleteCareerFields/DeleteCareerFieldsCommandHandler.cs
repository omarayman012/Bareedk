
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Vehicles;

namespace BaridikExpress.Application.Features.CareerFields.Commands.DeleteCareerFields;

    public class DeleteCareerFieldsCommandHandler(
        IGenericRepository<CareerField> repo,
        IStringLocalizer localizer ): IRequestHandler< DeleteCareerFieldsCommand,Result<bool>>
    {
        private readonly IGenericRepository<CareerField> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<bool>> Handle(
            DeleteCareerFieldsCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
                return Result<bool>.Failure( _localizer["InvalidIds"], 400 );

            var careerFields = await _repo.Query().Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!careerFields.Any())
                return Result<bool>.Failure( _localizer["CareerFieldsNotFound"],   404 );

        if (careerFields.Count != request.Ids.Count)
            return Result<bool>.Failure(
                  _localizer["SomeCareerFieldsNotFound"], 404);

        await _repo.DeleteRangeAsync(careerFields);

            return Result<bool>.Success(
               true,
                _localizer["OperationCompletedSuccessfully"]
            );
        }
    }

