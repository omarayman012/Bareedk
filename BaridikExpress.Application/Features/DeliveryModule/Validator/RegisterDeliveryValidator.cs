using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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

            // ================= BASIC INFO =================
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3).WithMessage(_localizer["FullNameMinLength"])
                .MaximumLength(150).WithMessage(_localizer["FullNameMaxLength"]);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(_localizer["DateOfBirthRequired"])
                .Must(BeValidAge).WithMessage(_localizer["InvalidAge"]);

            // ================= EMAIL =================
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .Must(BeValidEmail).WithMessage(_localizer["InvalidEmail"])
                .MaximumLength(256).WithMessage(_localizer["EmailMaxLength"])
                .MustAsync(EmailNotExists).WithMessage(_localizer["EmailAlreadyExists"]);

            // ================= PASSWORD =================
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer["PasswordRequired"])
                .Must(BeStrongPassword).WithMessage(_localizer["PasswordInvalid"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_localizer["ConfirmPasswordRequired"])
                .Equal(x => x.Password).WithMessage(_localizer["PasswordNotMatch"]);

            // ================= PHONE =================
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .Must(BeValidPhone).WithMessage(_localizer["InvalidPhone"])
                .MustAsync(PhoneNotExists).WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= VEHICLE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty().WithMessage(_localizer["PlateRequired"])
                .MaximumLength(20).WithMessage(_localizer["PlateMaxLength"]);

            RuleFor(x => x.VehType)
                .IsInEnum().WithMessage(_localizer["InvalidVehicleType"]);

            // ================= ADDRESS =================
            RuleFor(x => x.Country).MaximumLength(100);
            RuleFor(x => x.Gov).MaximumLength(100);
            RuleFor(x => x.City).MaximumLength(100);
            RuleFor(x => x.Village).MaximumLength(100);
            RuleFor(x => x.Address).MaximumLength(300);
            RuleFor(x => x.Floor).MaximumLength(20);
            RuleFor(x => x.Apt).MaximumLength(20);
            RuleFor(x => x.Edu).MaximumLength(150);

            // ================= FILES =================
            RuleFor(x => x.ProfileImg)
                .NotEmpty().WithMessage(_localizer["ProfileImageRequired"])
                .Must(BeValidImage).WithMessage(_localizer["InvalidImage"]);

            RuleFor(x => x.NidImg)
                .NotEmpty().WithMessage(_localizer["NationalIdRequired"])
                .Must(BeValidImage);

            RuleFor(x => x.LicImg)
                .NotEmpty().WithMessage(_localizer["LicenseRequired"])
                .Must(BeValidImage);

            RuleFor(x => x.VehImg)
                .NotEmpty().WithMessage(_localizer["VehicleImageRequired"])
                .Must(BeValidImage);

            RuleFor(x => x.PoliceCertImg)
                .NotEmpty().WithMessage(_localizer["PoliceCertRequired"])
                .Must(BeValidImage);

            RuleFor(x => x.PlateImg)
                .NotEmpty().WithMessage(_localizer["PlateImageRequired"])
                .Must(BeValidImage);

            // ================= TERMS =================
            RuleFor(x => x.TermsAccepted)
                .Equal(true).WithMessage(_localizer["MustAcceptTerms"]);

            RuleFor(x => x.PrivacyAccepted)
                .Equal(true).WithMessage(_localizer["MustAcceptPrivacy"]);
        }

        // ================= HELPERS =================

        private bool BeValidAge(DateTime dob)
            => dob <= DateTime.Today.AddYears(-18);

        private bool BeValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool BeValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone, @"^\+\d{5,20}$");
        }

        private bool BeStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= 8
                && password.Any(char.IsUpper)
                && password.Any(char.IsLower)
                && password.Any(char.IsDigit)
                && password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private bool BeValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(ext);
        }

        private async Task<bool> EmailNotExists(string email, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.Email == email, ct);

        private async Task<bool> PhoneNotExists(string phone, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.PhoneNumber == phone, ct);
    }
}