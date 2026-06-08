using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Addresses;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.DeleteAddresses;

public class DeleteAddressesCommandHandler(
    IGenericRepository<ClientAddress> repo,
    IGenericRepository<User> userRepo,
    IGetCurrentUserRepository currentUserRepository,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteAddressesCommand, Result<bool>>
{
    private readonly IGenericRepository<ClientAddress> _repo = repo;
    private readonly IGenericRepository<User> _userRepo = userRepo;
    private readonly IGetCurrentUserRepository _currentUserRepository = currentUserRepository;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteAddressesCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserRepository.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<bool>.Failure(
                _localizer["Unauthorized"],
                401);
        }

        var user = await _userRepo.FirstOrDefaultAsync(
            x => x.Id == userId,
            cancellationToken);

        if (user is null)
        {
            return Result<bool>.Failure(
                _localizer["UserNotFound"],
                404);
        }

        if (request.Ids is null || !request.Ids.Any())
        {
            return Result<bool>.Failure(
                _localizer["InvalidIds"],
                400);
        }

        var addresses = await _repo.Query()
            .Where(x =>
                x.UserId == userId &&
                request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!addresses.Any())
        {
            return Result<bool>.Failure(
                _localizer["AddressNotFound"],
                404);
        }

        if (addresses.Count != request.Ids.Count)
        {
            return Result<bool>.Failure(
                _localizer["SomeAddressesNotFound"],
                404);
        }

        await _repo.DeleteRangeAsync(addresses);

        return Result<bool>.Success(
            true,
            _localizer["OperationCompletedSuccessfully"]);
    }
}