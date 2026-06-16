using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class RegisterDeliveryHandler
        : IRequestHandler<RegisterDeliveryCommand, Result<RegisterDeliveryResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorage;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterDeliveryHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IFileStorageService fileStorage,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<RegisterDeliveryResponseDto>> Handle(
            RegisterDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= CHECK EMAIL =================
            var emailExists = await _userManager.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["EmailAlreadyExists"], 400);

            // ================= CHECK PHONE =================
            var phoneExists = await _userManager.Users
                .AnyAsync(x => x.PhoneNumber == request.Phone, cancellationToken);

            if (phoneExists)
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["PhoneAlreadyExists"], 400);

            // ================= CREATE USER =================
            var userEntity = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(userEntity, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(" , ",
                    createUserResult.Errors.Select(x => x.Description));

                return Result<RegisterDeliveryResponseDto>.Failure(errors, 400);
            }

            // ================= SAVE FILES =================
            string? profileImg = null;
            string? nidImg = null;
            string? licImg = null;
            string? vehImg = null;
            string? policeImg = null;
            string? plateImg = null;

            if (request.ProfileImg != null)
                profileImg = await _fileStorage.SaveFileAsync(
                    request.ProfileImg.OpenReadStream(),
                    request.ProfileImg.FileName,
                    "deliveries/profile");

            if (request.NidImg != null)
                nidImg = await _fileStorage.SaveFileAsync(
                    request.NidImg.OpenReadStream(),
                    request.NidImg.FileName,
                    "deliveries/nid");

            if (request.LicImg != null)
                licImg = await _fileStorage.SaveFileAsync(
                    request.LicImg.OpenReadStream(),
                    request.LicImg.FileName,
                    "deliveries/license");

            if (request.VehImg != null)
                vehImg = await _fileStorage.SaveFileAsync(
                    request.VehImg.OpenReadStream(),
                    request.VehImg.FileName,
                    "deliveries/vehicle");

            if (request.PoliceCertImg != null)
                policeImg = await _fileStorage.SaveFileAsync(
                    request.PoliceCertImg.OpenReadStream(),
                    request.PoliceCertImg.FileName,
                    "deliveries/police");

            if (request.PlateImg != null)
                plateImg = await _fileStorage.SaveFileAsync(
                    request.PlateImg.OpenReadStream(),
                    request.PlateImg.FileName,
                    "deliveries/plate");

            // ================= CREATE DELIVERY =================
            var delivery = new Delivery
            {
                UserId = userEntity.Id,

                DateOfBirth = request.DateOfBirth,

                PlateNo = request.PlateNo,
                VehType = request.VehType,

                CountryId = request.Country,
                GovernmentId = request.Gov,
                CityId = request.City,
                VillageId = request.Village,
                Address = request.Address,
                Floor = request.Floor,
                Apt = request.Apt,

                Edu = request.Edu,

                ProfileImg = profileImg,
                NidImg = nidImg,
                LicImg = licImg,
                VehImg = vehImg,
                PoliceCertImg = policeImg,
                PlateImg = plateImg,

                TermsAccepted = request.TermsAccepted,
                PrivacyAccepted = request.PrivacyAccepted,

                IsApproved = false,
                CreateType = DeliveryCreationType.External
            };

            await _context.Deliveries.AddAsync(delivery, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE =================
            var response = new RegisterDeliveryResponseDto
            {
                Id = delivery.UserId,

                FullName = request.FullName,
                DateOfBirth = delivery.DateOfBirth,

                PlateNo = delivery.PlateNo,
                VehType = delivery.VehType.ToString(),

                IsApproved = delivery.IsApproved,
                ApprovedAt = delivery.ApprovedAt,
                CreateType = delivery.CreateType.ToString(),

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

                Edu = delivery.Edu,

                ProfileImg = profileImg,
                NidImg = nidImg,
                LicImg = licImg,
                VehImg = vehImg,
                PoliceCertImg = policeImg,
                PlateImg = plateImg,

                TermsAccepted = delivery.TermsAccepted,
                PrivacyAccepted = delivery.PrivacyAccepted,

                Email = userEntity.Email!,
                Phone = userEntity.PhoneNumber!
            };

            return Result<RegisterDeliveryResponseDto>.Success(
                response,
                _localizer["DeliveryCreatedSuccessfully"],
                201);
        }
    }
}