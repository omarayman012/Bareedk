using BaridikExpress.Application.Features.ClientModule.Commond;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Validator
{
    public class UpdateClientValidator : AbstractValidator<UpdateClientCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public UpdateClientValidator(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            // ================= FULL NAME =================
            RuleFor(x => x.Dto.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3).WithMessage(_localizer["FullNameMinLength"])
                .MaximumLength(150).WithMessage(_localizer["FullNameMaxLength"]);

            // ================= CAREER FIELD =================
            RuleFor(x => x.Dto.CareerFieldId)
                .NotEmpty().WithMessage(_localizer["CareerFieldRequired"])
                .MustAsync(CareerFieldExists)
                .WithMessage(_localizer["CareerFieldNotFound"]);

            // ================= COMPANY NAME =================
            RuleFor(x => x.Dto.CompanyName)
                .MaximumLength(200)
                .WithMessage(_localizer["CompanyNameMaxLength"]);

            // ================= COMPANY LINK =================
            RuleFor(x => x.Dto.CompanyLink)
                .MaximumLength(500)
                .WithMessage(_localizer["CompanyLinkMaxLength"])
                .Must(BeValidUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.CompanyLink))
                .WithMessage(_localizer["InvalidCompanyLink"]);
        }

        // ================= HELPERS =================

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private async Task<bool> CareerFieldExists(Guid careerFieldId, CancellationToken ct)
        {
            return await _context.CareerFields
                .AnyAsync(x => x.Id == careerFieldId, ct);
        }
    }
}