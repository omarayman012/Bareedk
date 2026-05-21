using BaridikExpress.Application.Features.DeliveryModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // ================= FULL NAME =================
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3)
                .WithMessage(_localizer["FullNameMinLength"])
                .MaximumLength(150)
                .WithMessage(_localizer["FullNameMaxLength"]);

            // ================= DATE OF BIRTH =================
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage(_localizer["DateOfBirthRequired"])
                .Must(BeValidAge)
                .WithMessage(_localizer["InvalidAge"]);

            // ================= EMAIL =================
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_localizer["EmailRequired"])
                .EmailAddress()
                .WithMessage(_localizer["InvalidEmail"])
                .MustAsync(EmailNotExists)
                .WithMessage(_localizer["EmailAlreadyExists"]);

            // ================= PHONE =================
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage(_localizer["PhoneRequired"])
                .Must(BeValidPhone)
                .WithMessage(_localizer["InvalidPhone"])
                .MustAsync(PhoneNotExists)
                .WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= PASSWORD =================
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_localizer["PasswordRequired"])
                .MinimumLength(8)
                .WithMessage(_localizer["PasswordMinLength"]);

            // ================= PLATE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty()
                .WithMessage(_localizer["PlateRequired"])
                .MaximumLength(20)
                .WithMessage(_localizer["PlateMaxLength"]);

            // ================= VEHICLE TYPE =================
            RuleFor(x => x.VehType)
                .IsInEnum()
                .WithMessage(_localizer["InvalidVehicleType"]);

            // ================= OPTIONAL ADDRESS =================
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
                .NotNull()
                .WithMessage(_localizer["ProfileImageRequired"])
                .Must(BeValidImage)
                .WithMessage(_localizer["InvalidImage"]);

            RuleFor(x => x.NidImg)
                .NotNull()
                .Must(BeValidImage);

            RuleFor(x => x.LicImg)
                .NotNull()
                .Must(BeValidImage);

            RuleFor(x => x.VehImg)
                .NotNull()
                .Must(BeValidImage);

            RuleFor(x => x.PoliceCertImg)
                .NotNull()
                .Must(BeValidImage);

            RuleFor(x => x.PlateImg)
                .NotNull()
                .Must(BeValidImage);
        }

        // ================= HELPERS =================

        private bool BeValidAge(DateTime dob)
            => dob <= DateTime.Today.AddYears(-18);

        private bool BeValidPhone(string phone)
      => System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+\d{5,20}$");
        private bool BeValidImage(IFormFile file)
        {
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            return allowed.Contains(ext);
        }

        private async Task<bool> EmailNotExists(string email, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.Email == email, ct);

        private async Task<bool> PhoneNotExists(string phone, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(x => x.PhoneNumber == phone, ct);
    }
}
