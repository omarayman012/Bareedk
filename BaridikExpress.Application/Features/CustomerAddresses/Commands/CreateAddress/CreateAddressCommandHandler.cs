using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Domain.Entities.Customers;

namespace BaridikExpress.Application.Features.CustomerAddresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(
    IGenericRepository<BaridikExpress.Domain.Entities.Customers.Customer> customerRepo,
    IGenericRepository<CustomerAddress> addressRepo,
    IStringLocalizer localizer,
    IMapService mapService,
    IGetCurrentUserRepository currentUserRepository
) : IRequestHandler<CreateAddressCommand, Result<Guid>>
{
    private readonly IGenericRepository<BaridikExpress.Domain.Entities.Customers.Customer> _customerRepo = customerRepo;
    private readonly IGenericRepository<CustomerAddress> _addressRepo = addressRepo;
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

        var customer = await _customerRepo.FirstOrDefaultAsync(
            x => x.UserId == userId,
            cancellationToken);

        if (customer is null)
        {
            return Result<Guid>.Failure(
                _localizer["CustomerNotFound"],
                404);
        }

        string? location = null;

        if (request.Latitude.HasValue &&
            request.Longitude.HasValue)
        {
            location =
                await _mapService.GetAddressFromCoordinatesAsync(
                    request.Latitude.Value,
                    request.Longitude.Value,
                    cancellationToken);
        }

        if (request.IsDefault)
        {
            var defaultAddresses =
                await _addressRepo.FindAsync(
                    x => x.CustomerId == customer.Id &&
                         x.IsDefault,
                    cancellationToken);

            foreach (var address in defaultAddresses)
            {
                address.UpdateCustomerAddress(
                    isDefault: false);
            }

            _addressRepo.UpdateRange(defaultAddresses);

        }

        var newAddress = CustomerAddress.CreateAddress(
            customerId: customer.Id,
            addressType: request.AddressType,

            recipientName: request.RecipientName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,

            addressTitle: request.AddressTitle,
            apartmentNumber: request.ApartmentNumber,

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