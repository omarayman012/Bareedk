using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
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
    public class CreateDeliveryByAdminValidator
       : AbstractValidator<CreateDeliveryByAdminCommand>
    {
        private readonly UserManager<User> _userManager;

        public CreateDeliveryByAdminValidator(UserManager<User> userManager)
        {
            _userManager = userManager;

            // ================= BASIC INFO =================

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(100);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(BeAtLeast18YearsOld)
                .WithMessage("User must be at least 18 years old");

            // ================= VEHICLE =================

            RuleFor(x => x.PlateNo)
                .NotEmpty().WithMessage("Plate number is required")
                .MaximumLength(20);

            RuleFor(x => x.VehType)
                .IsInEnum().WithMessage("Invalid vehicle type");

            // ================= ADDRESS (optional but safe rules) =================

            RuleFor(x => x.Country)
                .MaximumLength(100);

            RuleFor(x => x.Gov)
                .MaximumLength(100);

            RuleFor(x => x.City)
                .MaximumLength(100);

            RuleFor(x => x.Village)
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .MaximumLength(200);

            RuleFor(x => x.Floor)
                .MaximumLength(20);

            RuleFor(x => x.Apt)
                .MaximumLength(20);

            // ================= FILES =================

            RuleFor(x => x.ProfileImg)
                .NotNull().WithMessage("Profile image is required");

            RuleFor(x => x.NidImg)
                .NotNull().WithMessage("National ID image is required");

            RuleFor(x => x.LicImg)
                .NotNull().WithMessage("License image is required");

            RuleFor(x => x.PlateImg)
                .NotNull().WithMessage("Plate image is required");

            // VehImg & PoliceCert optional (حسب كودك الحالي)
            RuleFor(x => x.VehImg)
                .NotNull().WithMessage("Vehicle image is required");

            RuleFor(x => x.PoliceCertImg)
                .NotNull().WithMessage("Police certificate image is required");

            // ================= AUTH =================

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(BeUniqueEmail)
                .WithMessage("Email already exists");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MinimumLength(8)
                .MustAsync(BeUniquePhone)
                .WithMessage("Phone already exists");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters");
        }

        // ================= AGE VALIDATION =================

        private bool BeAtLeast18YearsOld(DateTime date)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - date.Year;

            if (date.Date > today.AddYears(-age)) age--;

            return age >= 18;
        }

        // ================= UNIQUE CHECKS =================

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userManager.Users
                .AnyAsync(x => x.Email == email, cancellationToken);
        }

        private async Task<bool> BeUniquePhone(string phone, CancellationToken cancellationToken)
        {
            return !await _userManager.Users
                .AnyAsync(x => x.PhoneNumber == phone, cancellationToken);
        }
    }
}