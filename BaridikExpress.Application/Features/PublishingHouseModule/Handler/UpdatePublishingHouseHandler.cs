using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using BaridikExpress.Application.Interfaces.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Handler
{

    public class UpdatePublishingHouseHandler
        : IRequestHandler<UpdatePublishingHouseCommand, Result<UpdatePublishingHouseResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGetCurrentUserRepository _getCurrentUser;
        private readonly IStringLocalizer _localizer;
        private readonly IFileStorageService _fileService;
        private readonly IBaseUrlService _baseUrlService;

        public UpdatePublishingHouseHandler(
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

        public async Task<Result<UpdatePublishingHouseResponseDto>> Handle(
            UpdatePublishingHouseCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _getCurrentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return Result<UpdatePublishingHouseResponseDto>.Error(
                        _localizer["Unauthorized"], 401);

                var currentUser = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.Id == currentUserId, cancellationToken);

                if (currentUser == null)
                    return Result<UpdatePublishingHouseResponseDto>.Failure(
                        _localizer["UserNotFound"], 404);

                var entity = await _context.PublishingHouses
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity == null)
                    return Result<UpdatePublishingHouseResponseDto>.Failure(
                        _localizer["NotFound"], 404);

                // ===== Duplicate Check =====
                var isDuplicate = await _context.PublishingHouses.AnyAsync(x =>
                        x.Id != request.Id &&
                        (
                            x.Code == request.Code ||
                            x.EmailAddress == request.EmailAddress ||
                            x.PhoneNumber == request.PhoneNumber ||
                            x.NameAr == request.NameAr ||
                            x.NameEn == request.NameEn
                        ),
                    cancellationToken);

                if (isDuplicate)
                    return Result<UpdatePublishingHouseResponseDto>.Failure(
                        _localizer["PublishingHouseAlreadyExists"], 409);

                // ===== Name Handling =====
                var nameAr = request.NameAr?.Trim();
                var nameEn = request.NameEn?.Trim();

                if (string.IsNullOrWhiteSpace(nameAr) &&
                    string.IsNullOrWhiteSpace(nameEn))
                {
                    return Result<UpdatePublishingHouseResponseDto>.Failure(
                        _localizer["NameRequired"], 400);
                }

                if (string.IsNullOrWhiteSpace(nameEn))
                    nameEn = nameAr;

                if (string.IsNullOrWhiteSpace(nameAr))
                    nameAr = nameEn;

                // ===== Update Image if provided =====
                string oldImagePath = entity.LogoImage;
                if (request.Image != null)
                {
                    using var stream = request.Image.OpenReadStream();
                    string newImagePath = await _fileService.SaveFileAsync(
                        stream,
                        request.Image.FileName,
                        "publishing-houses");

                    if (!string.IsNullOrEmpty(newImagePath))
                    {
                        entity.LogoImage = newImagePath;

                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(oldImagePath))
                        {
                            await _fileService.DeleteFileAsync(oldImagePath);
                        }
                    }
                }

                // ===== Update Entity =====
                entity.Code = request.Code;
                entity.NameAr = nameAr!;
                entity.NameEn = nameEn!;
                entity.EmailAddress = request.EmailAddress;
                entity.PhoneNumber = request.PhoneNumber;
                entity.WebsiteLink = request.WebsiteLink;
                entity.Address = request.Address;

                entity.CountryId = request.CountryId;
                entity.GovernmentId = request.GovernmentId;
                entity.CityId = request.CityId;
                entity.VillageId = request.VillageId;

                entity.Street = request.Street;
                entity.BuildingNumber = request.BuildingNumber;
                entity.FloorNumber = request.FloorNumber;
                entity.DistinctiveMark = request.DistinctiveMark;
                entity.ZipCode = request.ZipCode;

                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedById = currentUserId;

                await _context.SaveChangesAsync(cancellationToken);

                // ===== Localized Lookups =====
                var country = await _context.Countries
                    .Where(x => x.CountryId == entity.CountryId)
                    .Select(x => new LocalizedNameDto
                    {
                        Id = x.CountryId,
                        AR = x.CountryNameAr,
                        EN = x.CountryNameEn
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                var government = await _context.Governments
                    .Where(x => x.GovernmentId == entity.GovernmentId)
                    .Select(x => new LocalizedNameDto
                    {
                        Id = x.GovernmentId,
                        AR = x.GovernmentNameAr,
                        EN = x.GovernmentNameEn
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                var city = entity.CityId != null
                    ? await _context.Cities
                        .Where(x => x.CityId == entity.CityId)
                        .Select(x => new LocalizedNameDto
                        {
                            Id = x.CityId,
                            AR = x.CityNameAr,
                            EN = x.CityNameEn
                        })
                        .FirstOrDefaultAsync(cancellationToken)
                    : null;

                var village = entity.VillageId != null
                    ? await _context.Villages
                        .Where(x => x.VillageId == entity.VillageId)
                        .Select(x => new LocalizedNameDto
                        {
                            Id = x.VillageId,
                            AR = x.VillageNameAr,
                            EN = x.VillageNameEn
                        })
                        .FirstOrDefaultAsync(cancellationToken)
                    : null;

                // ===== Response =====
                var dto = new UpdatePublishingHouseResponseDto
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

                    Country = country,
                    Government = government,
                    City = city,
                    Village = village,

                    Street = entity.Street,
                    BuildingNumber = entity.BuildingNumber,
                    FloorNumber = entity.FloorNumber,
                    DistinctiveMark = entity.DistinctiveMark,
                    ZipCode = entity.ZipCode,

                    LogoImage = _baseUrlService.ToAbsoluteMediaUrl(entity.LogoImage),

                    UpdatedAt = entity.UpdatedAt,
                    UpdatedBy = currentUser.FullName
                };

                return Result<UpdatePublishingHouseResponseDto>.Success(
                    dto,
                    _localizer["PublishingHouseUpdatedSuccessfully"],
                    200);
            }
            catch (Exception ex)
            {
                return Result<UpdatePublishingHouseResponseDto>.Error(
                    _localizer["FailedToUpdatePublishingHouse", ex.Message],
                    500);
            }
        }
    }
}