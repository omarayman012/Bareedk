using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class CreateDeliveryByAdminHandler
     : IRequestHandler<CreateDeliveryByAdminCommand, Result<RegisterDeliveryResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorage;
        private readonly IStringLocalizer _localizer;

        public CreateDeliveryByAdminHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IFileStorageService fileStorage,
            IStringLocalizer localizer)
        {
            _context = context;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _localizer = localizer;
        }

        public async Task<Result<RegisterDeliveryResponseDto>> Handle(
            CreateDeliveryByAdminCommand request,
            CancellationToken cancellationToken)
        {
            // ================= CHECK EMAIL =================

            var emailExists = await _userManager.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["EmailAlreadyExists"],
                    400);
            }

            // ================= CHECK PHONE =================

            var phoneExists = await _userManager.Users
                .AnyAsync(x => x.PhoneNumber == request.Phone, cancellationToken);

            if (phoneExists)
            {
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["PhoneAlreadyExists"],
                    400);
            }

            // ================= CREATE USER =================

            var user = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(" , ",
                    createUserResult.Errors.Select(x => x.Description));

                return Result<RegisterDeliveryResponseDto>.Failure(
                    errors,
                    400);
            }

            // ================= SAVE FILES =================

            string? profileImg = null;
            string? nidImg = null;
            string? licImg = null;
            string? vehImg = null;
            string? policeImg = null;
            string? plateImg = null;

            if (request.ProfileImg != null)
            {
                profileImg = await _fileStorage.SaveFileAsync(
                    request.ProfileImg.OpenReadStream(),
                    request.ProfileImg.FileName,
                    "deliveries/profile");
            }

            if (request.NidImg != null)
            {
                nidImg = await _fileStorage.SaveFileAsync(
                    request.NidImg.OpenReadStream(),
                    request.NidImg.FileName,
                    "deliveries/nid");
            }

            if (request.LicImg != null)
            {
                licImg = await _fileStorage.SaveFileAsync(
                    request.LicImg.OpenReadStream(),
                    request.LicImg.FileName,
                    "deliveries/license");
            }

            if (request.VehImg != null)
            {
                vehImg = await _fileStorage.SaveFileAsync(
                    request.VehImg.OpenReadStream(),
                    request.VehImg.FileName,
                    "deliveries/vehicle");
            }

            if (request.PoliceCertImg != null)
            {
                policeImg = await _fileStorage.SaveFileAsync(
                    request.PoliceCertImg.OpenReadStream(),
                    request.PoliceCertImg.FileName,
                    "deliveries/police");
            }

            if (request.PlateImg != null)
            {
                plateImg = await _fileStorage.SaveFileAsync(
                    request.PlateImg.OpenReadStream(),
                    request.PlateImg.FileName,
                    "deliveries/plate");
            }

            // ================= CREATE DELIVERY =================

            var delivery = new Delivery
            {
                UserId = user.Id,

                DateOfBirth = request.DateOfBirth,

                // VEHICLE
                PlateNo = request.PlateNo,
                VehType = request.VehType,

                // ADDRESS
                Country = request.Country,
                Gov = request.Gov,
                City = request.City,
                Village = request.Village,
                Address = request.Address,
                Floor = request.Floor,
                Apt = request.Apt,

                // OPTIONAL
                Edu = request.Edu,

                // FILES
                ProfileImg = profileImg,
                NidImg = nidImg,
                LicImg = licImg,
                VehImg = vehImg,
                PoliceCertImg = policeImg,
                PlateImg = plateImg,

                // TERMS
                TermsAccepted = true,
                PrivacyAccepted = true,

                // APPROVAL
                IsApproved = true,
                ApprovedAt = DateTime.UtcNow,

                CreateType = DeliveryCreationType.Internal
            };

            await _context.Deliveries.AddAsync(
                delivery,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE =================

            var response = new RegisterDeliveryResponseDto
            {
                Id = delivery.UserId,


                FullName = user.FullName,

                Email = user.Email!,
                Phone = user.PhoneNumber!,

                DateOfBirth = delivery.DateOfBirth,

                // VEHICLE
                PlateNo = delivery.PlateNo,
                VehType = delivery.VehType.ToString(),

                // APPROVAL
                IsApproved = delivery.IsApproved,
                ApprovedAt = delivery.ApprovedAt,
                CreateType = delivery.CreateType.ToString(),

                // ADDRESS
                Country = delivery.Country,
                Gov = delivery.Gov,
                City = delivery.City,
                Village = delivery.Village,
                Address = delivery.Address,
                Floor = delivery.Floor,
                Apt = delivery.Apt,

                // OPTIONAL
                Edu = delivery.Edu,

                // FILES
                ProfileImg = delivery.ProfileImg,
                NidImg = delivery.NidImg,
                LicImg = delivery.LicImg,
                VehImg = delivery.VehImg,
                PoliceCertImg = delivery.PoliceCertImg,
                PlateImg = delivery.PlateImg,

                // TERMS
                TermsAccepted = delivery.TermsAccepted,
                PrivacyAccepted = delivery.PrivacyAccepted
            };

            return Result<RegisterDeliveryResponseDto>.Success(
                response,
                _localizer["DeliveryCreatedSuccessfully"],
                201);
        }
    }
}
