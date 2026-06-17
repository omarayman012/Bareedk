using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
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
                .NotEmpty()
                .Must(BeValidEmail)
                .MustAsync(EmailNotExists)
                .WithMessage(_localizer["EmailAlreadyExists"]);

            // ================= PHONE =================
            RuleFor(x => x.Phone)
                .NotEmpty()
                .Must(BeValidPhone)
                .MustAsync(PhoneNotExists)
                .WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= PASSWORD =================
            RuleFor(x => x.Password)
                .NotEmpty()
                .Must(BeStrongPassword)
                .WithMessage(_localizer["PasswordInvalid"]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(_localizer["PasswordNotMatch"]);

            // ================= VEHICLE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty();

            RuleFor(x => x.VehType)
                .IsInEnum();

            // ================= LOCATION VALIDATION =================

            RuleFor(x => x.Country)
                .NotNull().MustAsync(CountryExists);

            RuleFor(x => x.Gov)
                .NotNull().MustAsync(GovExists)
                .MustAsync(BeGovInCountry);

            RuleFor(x => x.City)
                .NotNull().MustAsync(CityExists)
                .MustAsync(BeCityInGov);

            RuleFor(x => x.Village)
                .NotNull().MustAsync(VillageExists)
                .MustAsync(BeVillageInCity);

            // ================= FILES =================
            RuleFor(x => x.ProfileImg).NotNull();
            RuleFor(x => x.NidImg).NotNull();
            RuleFor(x => x.LicImg).NotNull();
            RuleFor(x => x.VehImg).NotNull();
            RuleFor(x => x.PoliceCertImg).NotNull();
            RuleFor(x => x.PlateImg).NotNull();

            // ================= TERMS =================
            RuleFor(x => x.TermsAccepted).Equal(true);
            RuleFor(x => x.PrivacyAccepted).Equal(true);
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