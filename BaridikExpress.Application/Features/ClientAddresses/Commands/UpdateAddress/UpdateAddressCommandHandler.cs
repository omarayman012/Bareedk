using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Domain.Entities.Addresses;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(
    IGenericRepository<User> userRepo,
    IGenericRepository<ClientAddress> addressRepo,
    IStringLocalizer localizer,
    IMapService mapService,
    IGetCurrentUserRepository currentUserRepository)
    : IRequestHandler<UpdateAddressCommand, Result<bool>>
{
    private readonly IGenericRepository<User> _userRepo = userRepo;
    private readonly IGenericRepository<ClientAddress> _addressRepo = addressRepo;
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

        var user = await _userRepo.FirstOrDefaultAsync(
            x => x.Id == userId,
            cancellationToken);

        if (user is null)
        {
            return Result<bool>.Failure(
                _localizer["UserNotFound"],
                404);
        }

        var address = await _addressRepo.FirstOrDefaultAsync(
            x => x.Id == request.Id &&
                 x.UserId == userId,
            cancellationToken);

        if (address is null)
        {
            return Result<bool>.Failure(
                _localizer["AddressNotFound"],
                404);
        }

        string? location = address.Location;

        if (request.Latitude.HasValue &&
            request.Longitude.HasValue)
        {
            var existingAddress =
                await _addressRepo.FirstOrDefaultAsync(
                    x => x.Id != address.Id &&
                         x.UserId == userId &&
                         x.Latitude == request.Latitude &&
                         x.Longitude == request.Longitude,
                    cancellationToken);

            if (existingAddress is not null)
            {
                return Result<bool>.Failure(
                    _localizer["AddressAlreadyExists"],
                    409);
            }

            location = await _mapService.GetAddressFromCoordinatesAsync(
                request.Latitude.Value,
                request.Longitude.Value,
                cancellationToken);
        }

        if (request.IsDefault == true)
        {
            var defaultAddresses =
                await _addressRepo.FindAsync(
                    x => x.UserId == userId &&
                         x.IsDefault &&
                         x.Id != address.Id,
                    cancellationToken);

            foreach (var item in defaultAddresses)
            {
                item.UpdateAddress(
                    isDefault: false);

                await _addressRepo.UpdateAsync(
                    item,
                    cancellationToken);
            }
        }

        address.UpdateAddress(
            addressType: request.AddressType,
            recipientName: request.RecipientName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            addressTitle: request.AddressTitle,
            flatNumber: request.FlatNumber,
            latitude: request.Latitude,
            longitude: request.Longitude,
            countryId: request.CountryId,
            governmentId: request.GovernmentId,
            cityId: request.CityId,
            villageId: request.VillageId,
            street: request.Street,
            buildingNumber: request.BuildingNumber,
            floorNumber: request.FloorNumber,
            distinctiveMark: request.DistinctiveMark,
            zipCode: request.ZipCode,
            location: location,
            isDefault: request.IsDefault);

        await _addressRepo.UpdateAsync(
            address,
            cancellationToken);

        return Result<bool>.Success(
            true,
            _localizer["OperationCompletedSuccessfully"]);
    }
}