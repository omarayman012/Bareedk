using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Vehicles.Commands.DeleteVehicles
{
    public class DeleteVehicleCommandHandler(
        IGenericRepository<Vehicle> repo,
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        IStringLocalizer localizer
    ) : IRequestHandler<DeleteVehicleCommand, Result<bool>>
    {
        private readonly IGenericRepository<Vehicle> _repo = repo;
        private readonly IApplicationDbContext _context = context;
        private readonly IFileStorageService _fileStorage = fileStorage;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<bool>> Handle(
            DeleteVehicleCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
                return Result<bool>.Failure(
                    _localizer["InvalidIds"],400);

            var vehicles = await _repo.Query()
                .Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (vehicles.Count != request.Ids.Count)
              return Result<bool>.Failure(
                    _localizer["SomeVehiclesNotFound"], 404);
            await using var transaction =
             await _context.Database
                 .BeginTransactionAsync(cancellationToken);

            try
            {
                await _repo.DeleteRangeAsync(vehicles);
                await transaction.CommitAsync(cancellationToken);
                foreach (var vehicle in vehicles)
                {
                    if (!string.IsNullOrWhiteSpace(vehicle.ImageUrl))
                    {
                        await _fileStorage.DeleteFileAsync( vehicle.ImageUrl);
                    }
                }

                return Result<bool>.Success(
                    true,
                    _localizer["OperationCompletedSuccessfully"]);
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<bool>.Failure(ex.Message, 500);
            }
        }
    }
    }