using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Customers;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.DeleteAddresses;

public class DeleteAddressesCommandHandler(
    IGenericRepository<CustomerAddress> repo,
    IStringLocalizer localizer) : IRequestHandler<DeleteAddressesCommand, Result<bool>>
{
    private readonly IGenericRepository<CustomerAddress> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteAddressesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids == null || !request.Ids.Any())
            return Result<bool>.Failure(_localizer["InvalidIds"], 400);

        var addresses = await _repo.Query()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!addresses.Any())
            return Result<bool>.Failure(_localizer["CustomerAddressesNotFound"], 404);

        if (addresses.Count != request.Ids.Count)
            return Result<bool>.Failure(_localizer["SomeCustomerAddressesNotFound"], 404);

        await _repo.DeleteRangeAsync(addresses.ToList());

        return Result<bool>.Success(true, _localizer["OperationCompletedSuccessfully"]);
    }
}
