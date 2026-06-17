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

            // ================= LOCATION HIERARCHY =================

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

            // ================= FILES =================
            RuleFor(x => x.ProfileImg).NotNull();
            RuleFor(x => x.NidImg).NotNull();
            RuleFor(x => x.LicImg).NotNull();
            RuleFor(x => x.VehImg).NotNull();
            RuleFor(x => x.PoliceCertImg).NotNull();
            RuleFor(x => x.PlateImg).NotNull();

            // ================= AUTH =================
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(BeUniqueEmail)
                .WithMessage(_localizer["EmailAlreadyExists"]);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MinimumLength(8)
                .MustAsync(BeUniquePhone)
                .WithMessage(_localizer["PhoneAlreadyExists"]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);
        }

        // ================= AGE =================
        private bool BeAtLeast18YearsOld(DateTime date)
        {
            var age = DateTime.UtcNow.Year - date.Year;
            if (date.Date > DateTime.UtcNow.AddYears(-age)) age--;
            return age >= 18;
        }

        // ================= EMAIL / PHONE =================
        private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
        {
            return !await _context.ApplicationUsers.AnyAsync(x => x.Email == email, ct);
        }

        private async Task<bool> BeUniquePhone(string phone, CancellationToken ct)
        {
            return !await _context.ApplicationUsers.AnyAsync(x => x.PhoneNumber == phone, ct);
        }

        // ================= LOCATION CHECKS =================

        private async Task<bool> CountryExists(Guid? id, CancellationToken ct)
        {
            return id.HasValue &&
                   await _context.Countries
                       .AnyAsync(x => x.CountryId == id.Value, ct);
        }

        private async Task<bool> GovExists(Guid? id, CancellationToken ct)
        {
            return id.HasValue &&
                   await _context.Governments
                       .AnyAsync(x => x.GovernmentId == id.Value, ct);
        }

        private async Task<bool> CityExists(Guid? id, CancellationToken ct)
        {
            return id.HasValue &&
                   await _context.Cities
                       .AnyAsync(x => x.CityId == id.Value, ct);
        }

        private async Task<bool> VillageExists(Guid? id, CancellationToken ct)
        {
            return id.HasValue &&
                   await _context.Villages
                       .AnyAsync(x => x.VillageId == id.Value, ct);
        }

        // ================= HIERARCHY VALIDATION =================

        private async Task<bool> BeGovInCountry(
            CreateDeliveryByAdminCommand cmd,
            Guid? govId,
            CancellationToken ct)
        {
            if (!govId.HasValue || !cmd.Country.HasValue)
                return false;

            return await _context.Governments
                .AnyAsync(x =>
                    x.GovernmentId == govId.Value &&
                    x.CountryId == cmd.Country.Value,
                    ct);
        }

        private async Task<bool> BeCityInGov(
            CreateDeliveryByAdminCommand cmd,
            Guid? cityId,
            CancellationToken ct)
        {
            if (!cityId.HasValue || !cmd.Gov.HasValue)
                return false;

            return await _context.Cities
                .AnyAsync(x =>
                    x.CityId == cityId.Value &&
                    x.GovernmentId == cmd.Gov.Value,
                    ct);
        }

        private async Task<bool> BeVillageInCity(
            CreateDeliveryByAdminCommand cmd,
            Guid? villageId,
            CancellationToken ct)
        {
            if (!villageId.HasValue || !cmd.City.HasValue)
                return false;

            return await _context.Villages
                .AnyAsync(x =>
                    x.VillageId == villageId.Value &&
                    x.CityId == cmd.City.Value,
                    ct);
        }
    }
}