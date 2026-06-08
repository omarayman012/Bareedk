using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.ClientAddresses.DTOs;

namespace BaridikExpress.Application.Features.ClientAddresses.Queries.GetAllAddresses;

public class GetAllAddressesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IGetCurrentUserRepository currentUserRepository)
    : IRequestHandler<GetAllAddressesQuery, Result<PaginatedList<GetAllAddressesDto>>>
{
    private readonly IApplicationDbContext _db = db;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IGetCurrentUserRepository _currentUserRepository = currentUserRepository;

    public async Task<Result<PaginatedList<GetAllAddressesDto>>> Handle(
        GetAllAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserRepository.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result<PaginatedList<GetAllAddressesDto>>.Failure(
                _localizer["Unauthorized"],
                401);
        }

        var query = _db.ClientAddresses
            .Where(x => x.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                (x.RecipientName != null && x.RecipientName.Contains(search)) ||
                (x.Street != null && x.Street.Contains(search)) ||
                (x.BuildingNumber != null && x.BuildingNumber.Contains(search)));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedById))
        {
            query = query.Where(x => x.CreatedById == request.CreatedById);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);
        }
        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllAddressesDto
            {
                Id = x.Id,
                RecipientName = x.RecipientName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,

                AddressType = x.AddressType.ToString(),

                Country = new LocalizedEntityDto
                {
                    Id = x.CountryId,
                    Name = new LocalizeLang
                    {
                        AR = x.Country.CountryNameAr ?? string.Empty,
                        EN = x.Country.CountryNameEn ?? string.Empty
                    }
                },
                Government = new LocalizedEntityDto
                {
                    Id = x.GovernmentId,
                    Name = new LocalizeLang
                    {
                        AR = x.Government.GovernmentNameAr ?? string.Empty,
                        EN = x.Government.GovernmentNameEn ?? string.Empty
                    }
                },
                City = new LocalizedEntityDto
                {
                    Id = x.CityId,
                    Name = new LocalizeLang
                    {
                        AR = x.City.CityNameAr ?? string.Empty,
                        EN = x.City.CityNameEn ?? string.Empty
                    }
                },

                Village = x.VillageId.HasValue
                    ? new LocalizedEntityDto
                    {
                        Id = x.VillageId.Value,
                        Name = new LocalizeLang
                        {
                            AR = x.Village!.VillageNameAr ?? string.Empty,
                            EN = x.Village!.VillageNameEn ?? string.Empty
                        }
                    }
                    : null,

                Street = x.Street,
                BuildingNumber = x.BuildingNumber,
                FlatNumber = x.FlatNumber,
                FloorNumber = x.FloorNumber,
                DistinctiveMark = x.DistinctiveMark,
                ZipCode = x.ZipCode,

                Location = x.Location,
                Latitude = x.Latitude,
                Longitude = x.Longitude,

                IsDefault = x.IsDefault,

                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : string.Empty,

                CreatedAt = x.CreatedAt
            });

        var paginatedResult =
            await PaginatedList<GetAllAddressesDto>.CreateAsync(
                result,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<GetAllAddressesDto>>.Success(
            paginatedResult,
            _localizer["OperationCompletedSuccessfully"]);
    }
}