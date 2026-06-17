using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.DeliveryModule;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class CreateDeliveryByAdminHandler
        : IRequestHandler<CreateDeliveryByAdminCommand, Result<RegisterDeliveryResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorage;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateDeliveryByAdminHandler(
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
            CreateDeliveryByAdminCommand request,
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
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
            {
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["EmailAlreadyExists"],
                    400);
            }

            // ================= CHECK PHONE =================
            if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.Phone, cancellationToken))
            {
                return Result<RegisterDeliveryResponseDto>.Failure(
                    _localizer["PhoneAlreadyExists"],
                    400);
            }

            // ================= CREATE USER =================
            var userEntity = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(
                userEntity,
                request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ",
                    createUserResult.Errors.Select(x => x.Description));

                return Result<RegisterDeliveryResponseDto>.Failure(errors, 400);
            }

            // ================= ASSIGN ROLE =================
            var addRoleResult = await _userManager.AddToRoleAsync(
                userEntity,
                "Delivery");

            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(", ",
                    addRoleResult.Errors.Select(x => x.Description));

                await _userManager.DeleteAsync(userEntity);

                return Result<RegisterDeliveryResponseDto>.Failure(errors, 400);
            }

            // ================= FILE UPLOADS =================
            async Task<string?> Save(IFormFile? file, string folder)
            {
                if (file == null) return null;

                return await _fileStorage.SaveFileAsync(
                    file.OpenReadStream(),
                    file.FileName,
                    folder);
            }

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

                ProfileImg = await Save(request.ProfileImg, "deliveries/profile"),
                NidImg = await Save(request.NidImg, "deliveries/nid"),
                LicImg = await Save(request.LicImg, "deliveries/license"),
                VehImg = await Save(request.VehImg, "deliveries/vehicle"),
                PoliceCertImg = await Save(request.PoliceCertImg, "deliveries/police"),
                PlateImg = await Save(request.PlateImg, "deliveries/plate"),

                TermsAccepted = true,
                PrivacyAccepted = true,

                IsApproved = true,
                ApprovedAt = DateTime.UtcNow,

                CreateType = DeliveryCreationType.Internal
            };

            await _context.Deliveries.AddAsync(delivery, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE =================
            var response = new RegisterDeliveryResponseDto
            {
                Id = delivery.UserId,
                FullName = userEntity.FullName,
                Email = userEntity.Email!,
                Phone = userEntity.PhoneNumber!,

                DateOfBirth = delivery.DateOfBirth,
                PlateNo = delivery.PlateNo,
                VehType = delivery.VehType.ToString(),

                IsApproved = delivery.IsApproved,
                ApprovedAt = delivery.ApprovedAt,
                CreateType = delivery.CreateType.ToString(),

                Country = await _context.Countries
                .Where(x => x.Id == delivery.CountryId)
                .Select(x => new LocalizedNameDto
                {
                    Id = x.Id,
                    EN = x.NameEn,
                    AR = x.NameAr
                })
                .FirstOrDefaultAsync(cancellationToken),

                Gov = await _context.Governments
                .Where(x => x.Id == delivery.GovernmentId)
                .Select(x => new LocalizedNameDto
                {
                    Id = x.Id,
                    EN = x.NameEn,
                    AR = x.NameAr
                })
                .FirstOrDefaultAsync(cancellationToken),

               City = delivery.CityId == null ? null :
                 await _context.Cities
                    .Where(x => x.Id == delivery.CityId)
                    .Select(x => new LocalizedNameDto
                    {
                        Id = x.Id,
                        EN = x.NameEn,
                        AR = x.NameAr
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                Village = delivery.VillageId == null ? null :
                 await _context.Villages
                    .Where(x => x.Id == delivery.VillageId)
                    .Select(x => new LocalizedNameDto
                    {
                        Id = x.Id,
                        EN = x.NameEn,
                        AR = x.NameAr
                    })
                    .FirstOrDefaultAsync(cancellationToken),
                Address = delivery.Address,
                Floor = delivery.Floor,
                Apt = delivery.Apt,

                Edu = delivery.Edu,

                ProfileImg = delivery.ProfileImg,
                NidImg = delivery.NidImg,
                LicImg = delivery.LicImg,
                VehImg = delivery.VehImg,
                PoliceCertImg = delivery.PoliceCertImg,
                PlateImg = delivery.PlateImg,

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