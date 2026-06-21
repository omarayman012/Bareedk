using BaridikExpress.Application.Features.DeliveryModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Validator
{
    public class UpdateDeliveryValidator : AbstractValidator<UpdateDeliveryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public UpdateDeliveryValidator(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            // ================= USER =================
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .Must(BeValidEmail).WithMessage(_localizer["InvalidEmail"])
                .MustAsync(BeUniqueEmail).WithMessage(_localizer["EmailAlreadyExists"]);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .Must(BeValidPhone).WithMessage(_localizer["InvalidPhone"])
                .MustAsync(BeUniquePhone).WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= VEHICLE =================
            RuleFor(x => x.PlateNo)
                .NotEmpty().WithMessage(_localizer["PlateNoRequired"]);

            RuleFor(x => x.VehType)
                .IsInEnum().WithMessage(_localizer["InvalidVehicleType"]);

            // ================= OPTIONAL FIELDS (NO REQUIRED RULES) =================
            // Country / Gov / City / Village / Address / Floor / Apt / Edu
            // ❌ لا نكتب NotEmpty ولا NotNull

            // ================= FILES (REQUIRED FROM ENTITY DESIGN) =================
            RuleFor(x => x.NidImg)
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
        }

        // ================= EMAIL =================
        private bool BeValidEmail(string email)
            => Regex.IsMatch(email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // ================= PHONE =================
        private bool BeValidPhone(string phone)
            => Regex.IsMatch(phone ?? "", @"^\+?\d{7,20}$");

        // ================= UNIQUE EMAIL =================
        private async Task<bool> BeUniqueEmail(
            UpdateDeliveryCommand request,
            string email,
            CancellationToken ct)
        {
            return !await _context.ApplicationUsers
                .AnyAsync(x => x.Email == email && x.Id != request.Id, ct);
        }

        // ================= UNIQUE PHONE =================
        private async Task<bool> BeUniquePhone(
            UpdateDeliveryCommand request,
            string phone,
            CancellationToken ct)
        {
            return !await _context.ApplicationUsers
                .AnyAsync(x => x.PhoneNumber == phone && x.Id != request.Id, ct);
        }

        // ================= FILE VALIDATION =================
        private bool BeValidFile(IFormFile file)
        {
            const long maxSize = 10 * 1024 * 1024;

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

            var ext = Path.GetExtension(file.FileName)?.ToLower();

            return file.Length <= maxSize && allowed.Contains(ext);
        }
    }
}
