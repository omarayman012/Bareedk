using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Domain.Entities.ClientModule;
using BaridikExpress.Domain.Entities.Customers;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(
    IGenericRepository<Client> ClientRepo,
    IGenericRepository<CustomerAddress> addressRepo,
    IStringLocalizer localizer,
    IMapService mapService,
    IGetCurrentUserRepository currentUserRepository
) : IRequestHandler<UpdateAddressCommand, Result<bool>>
{
    private readonly IGenericRepository<Client> _ClientRepo = ClientRepo;
    private readonly IGenericRepository<CustomerAddress> _addressRepo = addressRepo;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IMapService _mapService = mapService;
    private readonly IGetCurrentUserRepository _currentUserRepository = currentUserRepository;

    public async Task<Result<bool>> Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserRepository.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<bool>.Failure(
                _localizer["Unauthorized"],
                401);
        }

        var customer = await _ClientRepo.FirstOrDefaultAsync(
            x => x.UserId == userId,
            cancellationToken);

        if (customer is null)
        {
            return Result<bool>.Failure(
                _localizer["CustomerNotFound"],
                404);
        }

        var address = await _addressRepo.FirstOrDefaultAsync(
            x => x.Id == request.Id &&
                 x.CustomerId == customer.Id,
            cancellationToken);

        if (address is null)
        {
            return Result<bool>.Failure(
                _localizer["AddressNotFound"],
                404);
        }

        string? location = null;

        if (request.Latitude.HasValue &&
            request.Longitude.HasValue)
        {
            var existingAddress = await _addressRepo.FirstOrDefaultAsync(
               x => x.CustomerId != customer.Id &&
             x.Latitude == request.Latitude &&
             x.Longitude == request.Longitude, cancellationToken);

            if (existingAddress is not null)
                return Result<bool>.Failure(_localizer["AddressAlreadyExists"], 409);


            location = await _mapService.GetAddressFromCoordinatesAsync(
                    request.Latitude.Value,
                    request.Longitude.Value,
                    cancellationToken);
        }

        if (request.IsDefault == true)
        {
            var defaultAddresses =await _addressRepo.FindAsync(
                    x => x.CustomerId == customer.Id &&
                         x.IsDefault &&
                         x.Id != address.Id,
                    cancellationToken);

            foreach (var item in defaultAddresses)
            {
                item.UpdateCustomerAddress(
                    isDefault: false);

                await _addressRepo.UpdateAsync(
                    item,
                    cancellationToken);
            }
        }

        address.UpdateCustomerAddress(
            request.AddressType,
            request.CountryId,
            request.GovernmentId,
            request.CityId,
            request.VillageId,
            request.Street,
            request.BuildingNumber,
            request.FloorNumber,
            request.ApartmentNumber,
            request.DistinctiveMark,
            request.ZipCode,
            request.RecipientName,
            request.Email,
            request.PhoneNumber,
            request.AddressTitle,
            request.Latitude,
            request.Longitude,
            location,
            request.IsDefault);

        await _addressRepo.UpdateAsync(
            address,
            cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["OperationCompletedSuccessfully"]);
    }
}