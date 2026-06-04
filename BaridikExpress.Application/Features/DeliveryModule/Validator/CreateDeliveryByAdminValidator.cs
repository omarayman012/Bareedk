using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Validator
{
    public class CreateDeliveryByAdminValidator
        : AbstractValidator<CreateDeliveryByAdminCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public CreateDeliveryByAdminValidator(
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;

            // ================= BASIC INFO =================

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MaximumLength(100).WithMessage(_localizer["FullNameMaxLength"]);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(_localizer["DateOfBirthRequired"])
                .Must(BeAtLeast18YearsOld)
                .WithMessage(_localizer["InvalidAge"]);

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
            RuleFor(x => x.Address).MaximumLength(200);
            RuleFor(x => x.Floor).MaximumLength(20);
            RuleFor(x => x.Apt).MaximumLength(20);

            // ================= FILES =================

            RuleFor(x => x.ProfileImg)
                .NotNull().WithMessage(_localizer["ProfileImageRequired"]);

            RuleFor(x => x.NidImg)
                .NotNull().WithMessage(_localizer["NationalIdRequired"]);

            RuleFor(x => x.LicImg)
                .NotNull().WithMessage(_localizer["LicenseRequired"]);

            RuleFor(x => x.PlateImg)
                .NotNull().WithMessage(_localizer["PlateImageRequired"]);

            RuleFor(x => x.VehImg)
                .NotNull().WithMessage(_localizer["VehicleImageRequired"]);

            RuleFor(x => x.PoliceCertImg)
                .NotNull().WithMessage(_localizer["PoliceCertRequired"]);

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
                .MinimumLength(6)
                .WithMessage(_localizer["PasswordMinLength"]);
        }

        // ================= AGE VALIDATION =================

        private bool BeAtLeast18YearsOld(DateTime date)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - date.Year;

            if (date.Date > today.AddYears(-age))
                age--;

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