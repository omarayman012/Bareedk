using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.ClientAddresses.DTOs;

namespace BaridikExpress.Application.Features.ClientAddresses.Queries.GetAllAddresses;

public class GetAllAddressesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IGetCurrentUserRepository currentUserRepository
) : IRequestHandler<GetAllAddressesQuery, Result<PaginatedList<GetAllAddressesDto>>>
{
    private readonly IApplicationDbContext _db = db;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IGetCurrentUserRepository _currentUserRepository = currentUserRepository;

    public async Task<Result<PaginatedList<GetAllAddressesDto>>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserRepository.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
            return Result<PaginatedList<GetAllAddressesDto>>.Failure(_localizer["Unauthorized"], 401);

        var customer = await _db.Clients
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (customer is null)
            return Result<PaginatedList<GetAllAddressesDto>>.Failure(_localizer["CustomerNotFound"], 404);

        var query = _db.CustomerAddresses
            .Where(a => a.CustomerId == customer.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(a =>
                (a.RecipientName != null && a.RecipientName.Contains(search)) ||
                (a.Street != null && a.Street.Contains(search)) ||
                (a.BuildingNumber != null && a.BuildingNumber.Contains(search))
            );
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.CreatedById))
            query = query.Where(x => x.CreatedById == request.CreatedById);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);

        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllAddressesDto
            {
                Id = x.Id,
                RecipientName = x.RecipientName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                AddressType = x.AddressType != null ? x.AddressType.ToString() : null,
                Country = x.CountryId.HasValue ? new LocalizedEntityDto { Id = x.CountryId.Value, Name = new LocalizeLang { AR = x.Country!.CountryNameAr ?? string.Empty, EN = x.Country!.CountryNameEn ?? string.Empty } } : null,
                Government = x.GovernmentId.HasValue ? new LocalizedEntityDto { Id = x.GovernmentId.Value, Name = new LocalizeLang { AR = x.Government!.GovernmentNameAr ?? string.Empty, EN = x.Government!.GovernmentNameEn ?? string.Empty } } : null,
                City = x.CityId.HasValue ? new LocalizedEntityDto { Id = x.CityId.Value, Name = new LocalizeLang { AR = x.City!.CityNameAr ?? string.Empty, EN = x.City!.CityNameEn ?? string.Empty } } : null,
                Village = x.VillageId.HasValue ? new LocalizedEntityDto { Id = x.VillageId.Value, Name = new LocalizeLang { AR = x.Village!.VillageNameAr ?? string.Empty, EN = x.Village!.VillageNameEn ?? string.Empty } } : null,
                Street = x.Street,
                BuildingNumber = x.BuildingNumber,
                ApartmentNumber = x.ApartmentNumber,
                FloorNumber = x.FloorNumber,
                DistinctiveMark = x.DistinctiveMark,
                ZipCode = x.ZipCode,
                Location = x.Location,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsDefault = x.IsDefault,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : string.Empty,
                CreatedAt = x.CreatedAt
            });

        var paginatedResult = await PaginatedList<GetAllAddressesDto>.CreateAsync(result, request.PageNumber, request.PageSize);

        return Result<PaginatedList<GetAllAddressesDto>>.Success(paginatedResult, _localizer["OperationCompletedSuccessfully"]);
    }
}
