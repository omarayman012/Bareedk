using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.DeliveryModule.Validator
{
    public class CreateDeliveryByAdminValidator
        : AbstractValidator<CreateDeliveryByAdminCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public CreateDeliveryByAdminValidator(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            // ================= BASIC =================
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MaximumLength(100);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(_localizer["DateOfBirthRequired"])
                .Must(BeAtLeast18YearsOld)
                .WithMessage(_localizer["InvalidAge"]);

            // ================= VEHICLE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty().WithMessage(_localizer["PlateRequired"]);

            RuleFor(x => x.VehType)
                .IsInEnum();

            // ================= LOCATION =================
            RuleFor(x => x.Country)
                .NotNull().WithMessage(_localizer["CountryRequired"])
                .MustAsync(CountryExists)
                .WithMessage(_localizer["CountryNotFound"]);

            RuleFor(x => x.Gov)
                .NotNull().WithMessage(_localizer["GovRequired"])
                .MustAsync(GovExists)
                .WithMessage(_localizer["GovNotFound"])
                .MustAsync(BeGovInCountry)
                .WithMessage(_localizer["GovNotBelongToCountry"]);

            RuleFor(x => x.City)
                .NotNull().WithMessage(_localizer["CityRequired"])
                .MustAsync(CityExists)
                .WithMessage(_localizer["CityNotFound"])
                .MustAsync(BeCityInGov)
                .WithMessage(_localizer["CityNotBelongToGov"]);

            RuleFor(x => x.Village)
                .NotNull().WithMessage(_localizer["VillageRequired"])
                .MustAsync(VillageExists)
                .WithMessage(_localizer["VillageNotFound"])
                .MustAsync(BeVillageInCity)
                .WithMessage(_localizer["VillageNotBelongToCity"]);

            // ================= FILES (UPDATED STYLE) =================
            RuleFor(x => x.NidImg)
                           .NotNull().WithMessage(_localizer["ProfileImgRequired"])
                           .Must(BeValidFile).WithMessage(_localizer["InvalidProfileImg"]);

            RuleFor(x => x.ProfileImg)
                           .NotNull().WithMessage(_localizer["NidImgRequired"])
                           .Must(BeValidFile).WithMessage(_localizer["InvalidNidImg"]);

            RuleFor(x => x.LicImg)
                .NotNull().WithMessage(_localizer["LicenseImageRequired"])
                .Must(BeValidFile).WithMessage(_localizer["InvalidLicenseImage"]);

            RuleFor(x => x.VehImg)
                .NotNull().WithMessage(_localizer["VehicleImageRequired"])
                .Must(BeValidFile).WithMessage(_localizer["InvalidVehicleImage"]);

            RuleFor(x => x.PoliceCertImg)
                .NotNull().WithMessage(_localizer["PoliceCertificateImageRequired"])
                .Must(BeValidFile).WithMessage(_localizer["InvalidPoliceCertImage"]);

            RuleFor(x => x.PlateImg)
                .NotNull().WithMessage(_localizer["PlateImageRequired"])
                .Must(BeValidFile).WithMessage(_localizer["InvalidPlateImage"]);

            // ================= AUTH =================
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .EmailAddress().WithMessage(_localizer["InvalidEmail"])
                .MustAsync(BeUniqueEmail)
                .WithMessage(_localizer["EmailAlreadyExists"]);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .MinimumLength(8)
                .MustAsync(BeUniquePhone)
                .WithMessage(_localizer["PhoneAlreadyExists"]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer["PasswordRequired"])
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(_localizer["PasswordNotMatch"]);
        }

        // ================= AGE =================
        private bool BeAtLeast18YearsOld(DateTime date)
        {
            var age = DateTime.UtcNow.Year - date.Year;
            if (date.Date > DateTime.UtcNow.AddYears(-age)) age--;
            return age >= 18;
        }

        // ================= UNIQUE =================
        private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.Email == email, ct);

        private async Task<bool> BeUniquePhone(string phone, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.PhoneNumber == phone, ct);

        // ================= LOCATION =================
        private async Task<bool> CountryExists(Guid? id, CancellationToken ct)
            => id.HasValue && await _context.Countries.AnyAsync(x => x.CountryId == id.Value, ct);

        private async Task<bool> GovExists(Guid? id, CancellationToken ct)
            => id.HasValue && await _context.Governments.AnyAsync(x => x.GovernmentId == id.Value, ct);

        private async Task<bool> CityExists(Guid? id, CancellationToken ct)
            => id.HasValue && await _context.Cities.AnyAsync(x => x.CityId == id.Value, ct);

        private async Task<bool> VillageExists(Guid? id, CancellationToken ct)
            => id.HasValue && await _context.Villages.AnyAsync(x => x.VillageId == id.Value, ct);

        private async Task<bool> BeGovInCountry(CreateDeliveryByAdminCommand cmd, Guid? govId, CancellationToken ct)
        {
            if (!govId.HasValue || !cmd.Country.HasValue)
                return false;

            return await _context.Governments
                .AnyAsync(x => x.GovernmentId == govId.Value && x.CountryId == cmd.Country.Value, ct);
        }

        private async Task<bool> BeCityInGov(CreateDeliveryByAdminCommand cmd, Guid? cityId, CancellationToken ct)
        {
            if (!cityId.HasValue || !cmd.Gov.HasValue)
                return false;

            return await _context.Cities
                .AnyAsync(x => x.CityId == cityId.Value && x.GovernmentId == cmd.Gov.Value, ct);
        }

        private async Task<bool> BeVillageInCity(CreateDeliveryByAdminCommand cmd, Guid? villageId, CancellationToken ct)
        {
            if (!villageId.HasValue || !cmd.City.HasValue)
                return false;

            return await _context.Villages
                .AnyAsync(x => x.VillageId == villageId.Value && x.CityId == cmd.City.Value, ct);
        }
        private bool BeValidFile(IFormFile file)
        {
            if (file == null)
                return false;

            const long maxSize = 10 * 1024 * 1024; // 10 MB

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".pdf"
            };

            var extension = Path.GetExtension(file.FileName)?.ToLower();

            return file.Length <= maxSize &&
                   allowedExtensions.Contains(extension);
        }
    }
}