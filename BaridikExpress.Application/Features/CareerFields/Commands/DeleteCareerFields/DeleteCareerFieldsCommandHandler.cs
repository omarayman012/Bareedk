
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Commands.DeleteCareerFields;

    public class DeleteCareerFieldsCommandHandler(
        IGenericRepository<CareerField> repo,
        IStringLocalizer localizer ): IRequestHandler< DeleteCareerFieldsCommand,Result<List<Guid>>>
    {
        private readonly IGenericRepository<CareerField> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<List<Guid>>> Handle(
            DeleteCareerFieldsCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
                return Result<List<Guid>>.Failure( _localizer["InvalidIds"], 400 );

            var careerFields = await _repo.Query().Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!careerFields.Any())
                return Result<List<Guid>>.Failure( _localizer["CareerFieldsNotFound"],   404 );

            await _repo.DeleteRangeAsync(careerFields);

            return Result<List<Guid>>.Success(
                careerFields.Select(x => x.Id).ToList(),
                _localizer["OperationCompletedSuccessfully"]
            );
        }
    }

