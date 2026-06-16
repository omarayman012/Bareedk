using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.DeliveryModule.Command;
using BaridikExpress.Application.Features.DeliveryModule.DTOs;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class UpdateDeliveryHandler
        : IRequestHandler<UpdateDeliveryCommand, Result<UpdateDeliveryResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateDeliveryHandler(
            IApplicationDbContext context,
            IFileStorageService fileStorage,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _fileStorage = fileStorage;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<UpdateDeliveryResponseDto>> Handle(
            UpdateDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<UpdateDeliveryResponseDto>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= GET DELIVERY =================
            var delivery = await _context.Deliveries
                .Include(x => x.User)
                .Include(x => x.Country)
                .Include(x => x.Government)
                .Include(x => x.City)
                .Include(x => x.Village)
                .FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);

            if (delivery == null)
            {
                return Result<UpdateDeliveryResponseDto>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            // ================= USER =================
            delivery.User.FullName = request.FullName;

            // ================= DELIVERY DATA =================
            delivery.DateOfBirth = request.DateOfBirth;

            delivery.CountryId = request.Country;
            delivery.GovernmentId = request.Gov;
            delivery.CityId = request.City;
            delivery.VillageId = request.Village;

            delivery.Address = request.Address;
            delivery.Floor = request.Floor;
            delivery.Apt = request.Apt;

            delivery.PlateNo = request.PlateNo;
            delivery.VehType = request.VehType;

            // ================= FILES =================
            async Task<string?> SaveFile(IFormFile? file, string folder)
            {
                if (file == null) return null;

                return await _fileStorage.SaveFileAsync(
                    file.OpenReadStream(),
                    file.FileName,
                    folder);
            }

            if (request.NidImg != null)
                delivery.NidImg = await SaveFile(request.NidImg, "deliveries/nid");

            if (request.LicImg != null)
                delivery.LicImg = await SaveFile(request.LicImg, "deliveries/license");

            if (request.VehImg != null)
                delivery.VehImg = await SaveFile(request.VehImg, "deliveries/vehicle");

            if (request.PoliceCertImg != null)
                delivery.PoliceCertImg = await SaveFile(request.PoliceCertImg, "deliveries/police");

            if (request.PlateImg != null)
                delivery.PlateImg = await SaveFile(request.PlateImg, "deliveries/plate");

            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE =================
            var response = new UpdateDeliveryResponseDto
            {
                Id = delivery.Id,

                FullName = delivery.User.FullName,
                Email = delivery.User.Email!,
                Phone = delivery.User.PhoneNumber!,

                DateOfBirth = delivery.DateOfBirth,

                // LOCATION (Localized)
                Country = delivery.Country == null ? null : new LocalizedNameDto
                {
                    Id = delivery.Country.Id,
                    EN = delivery.Country.NameEn,
                    AR = delivery.Country.NameAr
                },

                Gov = delivery.Government == null ? null : new LocalizedNameDto
                {
                    Id = delivery.Government.Id,
                    EN = delivery.Government.NameEn,
                    AR = delivery.Government.NameAr
                },

                City = delivery.City == null ? null : new LocalizedNameDto
                {
                    Id = delivery.City.Id,
                    EN = delivery.City.NameEn,
                    AR = delivery.City.NameAr
                },

                Village = delivery.Village == null ? null : new LocalizedNameDto
                {
                    Id = delivery.Village.Id,
                    EN = delivery.Village.NameEn,
                    AR = delivery.Village.NameAr
                },

                Address = delivery.Address,
                Floor = delivery.Floor,
                Apt = delivery.Apt,

                // VEHICLE
                PlateNo = delivery.PlateNo,
                VehType = delivery.VehType.ToString(),

                // FILES
                NidImg = delivery.NidImg,
                LicImg = delivery.LicImg,
                VehImg = delivery.VehImg,
                PoliceCertImg = delivery.PoliceCertImg,
                PlateImg = delivery.PlateImg,

                // APPROVAL
                IsApproved = delivery.IsApproved,
                ApprovedAt = delivery.ApprovedAt,
                CreateType = delivery.CreateType.ToString()
            };

            return Result<UpdateDeliveryResponseDto>.Success(
                response,
                _localizer["DeliveryUpdatedSuccessfully"],
                200);
        }
    }
}