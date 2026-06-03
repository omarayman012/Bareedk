using BaridikExpress.Application.Features.DeliveryModule.Command;
using BaridikExpress.Application.Features.DeliveryModule.DTOs;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class UpdateDeliveryHandler
       : IRequestHandler<UpdateDeliveryCommand, Result<UpdateDeliveryResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorage;
        private readonly IStringLocalizer<UpdateDeliveryHandler> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateDeliveryHandler(
            IApplicationDbContext context,
            IFileStorageService fileStorage,
            IStringLocalizer<UpdateDeliveryHandler> localizer,
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
                .FirstOrDefaultAsync(
                    x => x.UserId == request.Id,
                    cancellationToken);

            if (delivery == null)
            {
                return Result<UpdateDeliveryResponseDto>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            // ================= USER =================
            delivery.User.FullName = request.FullName;

            // ================= DELIVERY =================
            delivery.DateOfBirth = request.DateOfBirth;

            delivery.Country = request.Country;
            delivery.Gov = request.Gov;
            delivery.City = request.City;
            delivery.Village = request.Village;
            delivery.Address = request.Address;
            delivery.Floor = request.Floor;
            delivery.Apt = request.Apt;

            delivery.PlateNo = request.PlateNo;
            delivery.VehType = request.VehType;

            // ================= FILES =================
            if (request.NidImg != null)
            {
                delivery.NidImg = await _fileStorage.SaveFileAsync(
                    request.NidImg.OpenReadStream(),
                    request.NidImg.FileName,
                    "deliveries/nid");
            }

            if (request.LicImg != null)
            {
                delivery.LicImg = await _fileStorage.SaveFileAsync(
                    request.LicImg.OpenReadStream(),
                    request.LicImg.FileName,
                    "deliveries/license");
            }

            if (request.VehImg != null)
            {
                delivery.VehImg = await _fileStorage.SaveFileAsync(
                    request.VehImg.OpenReadStream(),
                    request.VehImg.FileName,
                    "deliveries/vehicle");
            }

            if (request.PoliceCertImg != null)
            {
                delivery.PoliceCertImg = await _fileStorage.SaveFileAsync(
                    request.PoliceCertImg.OpenReadStream(),
                    request.PoliceCertImg.FileName,
                    "deliveries/police");
            }

            if (request.PlateImg != null)
            {
                delivery.PlateImg = await _fileStorage.SaveFileAsync(
                    request.PlateImg.OpenReadStream(),
                    request.PlateImg.FileName,
                    "deliveries/plate");
            }

            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE =================
            var response = new UpdateDeliveryResponseDto
            {
                Id = delivery.Id,

                FullName = delivery.User.FullName,
                Email = delivery.User.Email!,
                Phone = delivery.User.PhoneNumber!,

                DateOfBirth = delivery.DateOfBirth,

                Country = delivery.Country,
                Gov = delivery.Gov,
                City = delivery.City,
                Village = delivery.Village,
                Address = delivery.Address,
                Floor = delivery.Floor,
                Apt = delivery.Apt,

                PlateNo = delivery.PlateNo,
                VehType = delivery.VehType.ToString(),

                NidImg = delivery.NidImg,
                LicImg = delivery.LicImg,
                VehImg = delivery.VehImg,
                PoliceCertImg = delivery.PoliceCertImg,
                PlateImg = delivery.PlateImg,

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