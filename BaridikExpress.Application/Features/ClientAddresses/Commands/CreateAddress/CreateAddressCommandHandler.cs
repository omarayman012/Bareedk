using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Domain.Entities.Addresses;
using BaridikExpress.Domain.Entities.Location;

namespace BaridikExpress.Application.Features.ClientAddresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(
IGenericRepository<User> userRepo,
IGenericRepository<ClientAddress> addressRepo,
IGenericRepository<Country> countryRepo,
IGenericRepository<Government> governmentRepo,
IGenericRepository<City> cityRepo,
IStringLocalizer localizer,
IMapService mapService,
IGetCurrentUserRepository currentUserRepository)
: IRequestHandler<CreateAddressCommand, Result<Guid>>
{
    private readonly IGenericRepository<User> _userRepo = userRepo;
    private readonly IGenericRepository<ClientAddress> _addressRepo = addressRepo;
    private readonly IGenericRepository<Country> _countryRepo = countryRepo;
    private readonly IGenericRepository<Government> _governmentRepo = governmentRepo;
    private readonly IGenericRepository<City> _cityRepo = cityRepo;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IMapService _mapService = mapService;
    private readonly IGetCurrentUserRepository _currentUserRepository = currentUserRepository;

public async Task<Result<Guid>> Handle(
    CreateAddressCommand request,
    CancellationToken cancellationToken)
    {
        var userId = _currentUserRepository.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<Guid>.Failure(
                _localizer["Unauthorized"],
                401);
        }
        
        var user = await _userRepo.FirstOrDefaultAsync(x=>x.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result<Guid>.Failure(
                _localizer["UserNotFound"],
                404);
        }

        string? location = null;

        if (request.Latitude.HasValue &&
            request.Longitude.HasValue)
        {
            var existingAddress =
                await _addressRepo.FirstOrDefaultAsync(
                    x => x.UserId == userId &&
                         x.Latitude == request.Latitude &&
                         x.Longitude == request.Longitude,
                    cancellationToken);

            if (existingAddress is not null)
            {
                return Result<Guid>.Failure(
                    _localizer["AddressAlreadyExists"],
                    409);
            }

            location = await _mapService.GetAddressFromCoordinatesAsync(
                    request.Latitude.Value,
                    request.Longitude.Value,
                    cancellationToken);
        }

        if (request.IsDefault)
        {
            var defaultAddresses =
                await _addressRepo.FindAsync(
                    x => x.UserId == userId &&
                         x.IsDefault,
                    cancellationToken);

            foreach (var address in defaultAddresses)
            {
                address.UpdateAddress(
                    isDefault: false);
            }

            _addressRepo.UpdateRange(defaultAddresses);
        }

        var country = await _countryRepo
    .FirstOrDefaultAsync(x => x.Id == request.CountryId, cancellationToken);

        if (country is null)
        {
            return Result<Guid>.Failure(
                _localizer["CountryNotFound"],
                404);
        }


        var government = await _governmentRepo
    .FirstOrDefaultAsync(
        x => x.Id == request.GovernmentId
          && x.CountryId == request.CountryId,
        cancellationToken);

        if (government is null)
        {
            return Result<Guid>.Failure(
                _localizer["GovernmentNotBelongToCountry"],
                400);
        }

        var city = await _cityRepo
    .FirstOrDefaultAsync(
        x => x.Id == request.CityId
          && x.GovernmentId == request.GovernmentId,
        cancellationToken);

        if (city is null)
        {
            return Result<Guid>.Failure(
                _localizer["CityNotBelongToGovernment"],
                400);
        }




        var newAddress = ClientAddress.CreateAddress(
            userId: userId,
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

        await _addressRepo.AddAsync(
            newAddress,
            cancellationToken);

        return Result<Guid>.Success(
            newAddress.Id,
            _localizer["OperationCompletedSuccessfully"]);
    }

}
