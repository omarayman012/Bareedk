using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace BaridikExpress.Application.Features.DeliveryModule.Validator
{
    public class RegisterDeliveryValidator : AbstractValidator<RegisterDeliveryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public RegisterDeliveryValidator(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            // ================= BASIC =================
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3)
                .MaximumLength(150);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(BeValidAge)
                .WithMessage(_localizer["InvalidAge"]);

            // ================= EMAIL =================
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .Must(BeValidEmail)
                .WithMessage(_localizer["InvalidEmail"])
                .MustAsync(EmailNotExists)
                .WithMessage(_localizer["EmailAlreadyExists"]);

            // ================= PHONE =================
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .Must(BeValidPhone)
                .WithMessage(_localizer["InvalidPhone"])
                .MustAsync(PhoneNotExists)
                .WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= PASSWORD =================
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer["PasswordRequired"])
                .Must(BeStrongPassword)
                .WithMessage(_localizer["PasswordInvalid"]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(_localizer["PasswordNotMatch"]);

            // ================= VEHICLE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty()
                .WithMessage(_localizer["PlateNoRequired"]);

            RuleFor(x => x.VehType)
                .IsInEnum()
                .WithMessage(_localizer["InvalidVehicleType"]);

            // ================= LOCATION VALIDATION =================
            RuleFor(x => x.Country)
                .NotNull()
                .WithMessage(_localizer["CountryRequired"])
                .MustAsync(CountryExists)
                .WithMessage(_localizer["InvalidCountry"]);

            RuleFor(x => x.Gov)
                .NotNull()
                .WithMessage(_localizer["GovRequired"])
                .MustAsync(GovExists)
                .WithMessage(_localizer["InvalidGov"])
                .MustAsync(BeGovInCountry)
                .WithMessage(_localizer["GovNotInCountry"]);

            RuleFor(x => x.City)
                .NotNull()
                .WithMessage(_localizer["CityRequired"])
                .MustAsync(CityExists)
                .WithMessage(_localizer["InvalidCity"])
                .MustAsync(BeCityInGov)
                .WithMessage(_localizer["CityNotInGov"]);

            RuleFor(x => x.Village)
                .NotNull()
                .WithMessage(_localizer["VillageRequired"])
                .MustAsync(VillageExists)
                .WithMessage(_localizer["InvalidVillage"])
                .MustAsync(BeVillageInCity)
                .WithMessage(_localizer["VillageNotInCity"]);

            // ================= FILES =================
          
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

            // ================= TERMS =================
            RuleFor(x => x.TermsAccepted)
                .Equal(true)
                .WithMessage(_localizer["TermsMustBeAccepted"]);

            RuleFor(x => x.PrivacyAccepted)
                .Equal(true)
                .WithMessage(_localizer["PrivacyMustBeAccepted"]);
        }

        // ================= AGE =================
        private bool BeValidAge(DateTime dob)
            => dob <= DateTime.Today.AddYears(-18);

        // ================= EMAIL =================
        private bool BeValidEmail(string email)
            => Regex.IsMatch(email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // ================= PHONE =================
        private bool BeValidPhone(string phone)
            => Regex.IsMatch(phone ?? "", @"^\+?\d{7,20}$");

        // ================= PASSWORD =================
        private bool BeStrongPassword(string password)
        {
            return password?.Length >= 8
                && password.Any(char.IsUpper)
                && password.Any(char.IsLower)
                && password.Any(char.IsDigit)
                && password.Any(c => !char.IsLetterOrDigit(c));
        }

        // ================= FILE VALIDATION =================
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

        // ================= UNIQUE =================
        private async Task<bool> EmailNotExists(string email, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.Email == email, ct);

        private async Task<bool> PhoneNotExists(string phone, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.PhoneNumber == phone, ct);

        // ================= LOCATION EXISTS =================
        private async Task<bool> CountryExists(Guid? id, CancellationToken ct)
            => id != null && await _context.Countries.AnyAsync(x => x.Id == id, ct);

        private async Task<bool> GovExists(Guid? id, CancellationToken ct)
            => id != null && await _context.Governments.AnyAsync(x => x.Id == id, ct);

        private async Task<bool> CityExists(Guid? id, CancellationToken ct)
            => id != null && await _context.Cities.AnyAsync(x => x.Id == id, ct);

        private async Task<bool> VillageExists(Guid? id, CancellationToken ct)
            => id != null && await _context.Villages.AnyAsync(x => x.Id == id, ct);

        // ================= HIERARCHY =================
        private async Task<bool> BeGovInCountry(RegisterDeliveryCommand cmd, Guid? govId, CancellationToken ct)
        {
            return await _context.Governments
                .AnyAsync(x => x.Id == govId && x.CountryId == cmd.Country, ct);
        }

        private async Task<bool> BeCityInGov(RegisterDeliveryCommand cmd, Guid? cityId, CancellationToken ct)
        {
            return await _context.Cities
                .AnyAsync(x => x.Id == cityId && x.GovernmentId == cmd.Gov, ct);
        }

        private async Task<bool> BeVillageInCity(RegisterDeliveryCommand cmd, Guid? villageId, CancellationToken ct)
        {
            return await _context.Villages
                .AnyAsync(x => x.Id == villageId && x.CityId == cmd.City, ct);
        }
    }
}