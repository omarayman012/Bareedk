using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.PublishingHouseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Handler
{
    public class CreatePublishingHouseHandler
        : IRequestHandler<CreatePublishingHouseCommand, Result<CreatePublishingHouseResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGetCurrentUserRepository _getCurrentUser;
        private readonly IStringLocalizer _localizer;
        private readonly IFileStorageService _fileService;
        private readonly IBaseUrlService _baseUrlService;

        public CreatePublishingHouseHandler(
            IApplicationDbContext context,
            IGetCurrentUserRepository getCurrentUser,
            IStringLocalizer localizer,
            IFileStorageService fileService,
            IBaseUrlService baseUrlService)
        {
            _context = context;
            _getCurrentUser = getCurrentUser;
            _localizer = localizer;
            _fileService = fileService;
            _baseUrlService = baseUrlService;
        }

        public async Task<Result<CreatePublishingHouseResponseDto>> Handle(
       CreatePublishingHouseCommand request,
       CancellationToken cancellationToken)
        {
            try
            {
                var userId = _getCurrentUser.GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Result<CreatePublishingHouseResponseDto>.Error(_localizer["Unauthorized"], 401);

                var user = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

                if (user == null)
                    return Result<CreatePublishingHouseResponseDto>.Failure(_localizer["UserNotFound"], 404);

                // =========================
                // Validate Name
                // =========================
                var nameAr = request.NameAr?.Trim();
                var nameEn = request.NameEn?.Trim();

                if (string.IsNullOrWhiteSpace(nameAr) && string.IsNullOrWhiteSpace(nameEn))
                    return Result<CreatePublishingHouseResponseDto>.Failure(_localizer["NameRequired"], 400);

                nameEn ??= nameAr;
                nameAr ??= nameEn;

                // =========================
                // Validate Country
                // =========================
                var country = await _context.Countries
                    .FirstOrDefaultAsync(x => x.CountryId == request.CountryId, cancellationToken);

                if (country == null)
                    return Result<CreatePublishingHouseResponseDto>.Failure("Invalid Country", 400);

                // =========================
                // Validate Government + relation to Country
                // =========================
                var government = await _context.Governments
                    .FirstOrDefaultAsync(x => x.GovernmentId == request.GovernmentId, cancellationToken);

                if (government == null)
                    return Result<CreatePublishingHouseResponseDto>.Failure("Invalid Government", 400);

                if (government.CountryId != request.CountryId)
                    return Result<CreatePublishingHouseResponseDto>.Failure(
                        "Government does not belong to selected Country", 400);

                // =========================
                // Validate City (optional)
                // =========================
                City? city = null;

                if (request.CityId.HasValue)
                {
                    city = await _context.Cities
                        .FirstOrDefaultAsync(x => x.CityId == request.CityId.Value, cancellationToken);

                    if (city == null)
                        return Result<CreatePublishingHouseResponseDto>.Failure("Invalid City", 400);

                    if (city.GovernmentId != request.GovernmentId)
                        return Result<CreatePublishingHouseResponseDto>.Failure(
                            "City does not belong to selected Government", 400);
                }

                // =========================
                // Validate Village (optional)
                // =========================
                Village? village = null;

                if (request.VillageId.HasValue)
                {
                    village = await _context.Villages
                        .FirstOrDefaultAsync(x => x.VillageId == request.VillageId.Value, cancellationToken);

                    if (village == null)
                        return Result<CreatePublishingHouseResponseDto>.Failure("Invalid Village", 400);

                    if (!request.CityId.HasValue || village.CityId != request.CityId.Value)
                        return Result<CreatePublishingHouseResponseDto>.Failure(
                            "Village does not belong to selected City", 400);
                }

                // =========================
                // Upload Image
                // =========================
                string logoImagePath = string.Empty;

                if (request.Image != null)
                {
                    using var stream = request.Image.OpenReadStream();

                    logoImagePath = await _fileService.SaveFileAsync(
                        stream,
                        request.Image.FileName,
                        "publishing-houses");
                }

                // =========================
                // Create Entity
                // =========================
                var entity = new PublishingHouse
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    NameAr = nameAr!,
                    NameEn = nameEn!,
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    WebsiteLink = request.WebsiteLink,
                    Address = request.Address,

                    CountryId = request.CountryId,
                    GovernmentId = request.GovernmentId,
                    CityId = request.CityId,
                    VillageId = request.VillageId,

                    Street = request.Street,
                    BuildingNumber = request.BuildingNumber,
                    FloorNumber = request.FloorNumber,
                    DistinctiveMark = request.DistinctiveMark,
                    ZipCode = request.ZipCode,

                    Active = true,
                    LogoImage = logoImagePath,

                    CreatedAt = DateTime.UtcNow,
                    CreatedById = userId
                };

                _context.PublishingHouses.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                // =========================
                // Build DTO
                // =========================
                var dto = new CreatePublishingHouseResponseDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = new LocalizedDto
                    {
                        AR = entity.NameAr,
                        EN = entity.NameEn
                    },
                    EmailAddress = entity.EmailAddress,
                    PhoneNumber = entity.PhoneNumber,
                    WebsiteLink = entity.WebsiteLink,
                    Address = entity.Address,

                    Country = new LocalizedNameDto
                    {
                        Id = country.CountryId,
                        AR = country.CountryNameAr,
                        EN = country.CountryNameEn
                    },

                    Government = new LocalizedNameDto
                    {
                        Id = government.GovernmentId,
                        AR = government.GovernmentNameAr,
                        EN = government.GovernmentNameEn
                    },

                    City = city == null ? null : new LocalizedNameDto
                    {
                        Id = city.CityId,
                        AR = city.CityNameAr,
                        EN = city.CityNameEn
                    },

                    Village = village == null ? null : new LocalizedNameDto
                    {
                        Id = village.VillageId,
                        AR = village.VillageNameAr,
                        EN = village.VillageNameEn
                    },

                    Street = entity.Street,
                    BuildingNumber = entity.BuildingNumber,
                    FloorNumber = entity.FloorNumber,
                    DistinctiveMark = entity.DistinctiveMark,
                    ZipCode = entity.ZipCode,

                    Active = entity.Active,

                    LogoImage = _baseUrlService.ToAbsoluteMediaUrl(logoImagePath),

                    CreatedAt = entity.CreatedAt,
                    CreatedBy = user.FullName
                };

                return Result<CreatePublishingHouseResponseDto>.Success(
                    dto,
                    _localizer["PublishingHouseCreatedSuccessfully"],
                    201);
            }
            catch (Exception ex)
            {
                return Result<CreatePublishingHouseResponseDto>.Error(
                    _localizer["FailedToCreatePublishingHouse", ex.Message],
                    500);
            }
        }
    }
}
