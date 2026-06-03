using BaridikExpress.Application.Features.Auth.Command;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Validator
{
    public class RegisterClientValidator : AbstractValidator<RegisterClientCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public RegisterClientValidator(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            // ================= FULL NAME =================
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3).WithMessage(_localizer["FullNameMinLength"])
                .MaximumLength(150).WithMessage(_localizer["FullNameMaxLength"]);

            // ================= EMAIL =================
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .EmailAddress().WithMessage(_localizer["InvalidEmail"])
                .MaximumLength(256).WithMessage(_localizer["EmailMaxLength"])
                .MustAsync(EmailNotExists).WithMessage(_localizer["EmailAlreadyExists"]);

            // ================= PHONE =================
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .Matches(@"^\+\d{5,20}$").WithMessage(_localizer["InvalidPhone"])
                .MustAsync(PhoneNotExists).WithMessage(_localizer["PhoneAlreadyExists"]);

            // ================= PASSWORD =================
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer["PasswordRequired"])
                .Must(BeStrongPassword).WithMessage(_localizer["PasswordInvalid"]);

            // ================= CONFIRM PASSWORD =================
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_localizer["ConfirmPasswordRequired"])
                .Equal(x => x.Password).WithMessage(_localizer["PasswordNotMatch"]);

            // ================= CAREER FIELD =================
            RuleFor(x => x.CareerFieldId)
                .NotEmpty().WithMessage(_localizer["CareerFieldRequired"])
                .MustAsync(CareerFieldExists).WithMessage(_localizer["CareerFieldNotFound"]);

            // ================= COMPANY NAME =================
            RuleFor(x => x.CompanyName)
                .MaximumLength(200).WithMessage(_localizer["CompanyNameMaxLength"]);

            // ================= COMPANY LINK =================
            RuleFor(x => x.CompanyLink)
                .MaximumLength(500).WithMessage(_localizer["CompanyLinkMaxLength"])
                .Must(BeValidUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.CompanyLink))
                .WithMessage(_localizer["InvalidCompanyLink"]);

            // ================= TERMS =================
            RuleFor(x => x.AcceptTerms)
                .Equal(true).WithMessage(_localizer["MustAcceptTerms"]);

            // ================= PRIVACY =================
            RuleFor(x => x.AcceptPrivacy)
                .Equal(true).WithMessage(_localizer["MustAcceptPrivacy"]);
        }

        // ================= HELPERS =================
        private bool BeStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return true;

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private async Task<bool> EmailNotExists(string email, CancellationToken ct)
        {
            return !await _context.ApplicationUsers
                .AnyAsync(u => u.Email == email, ct);
        }

        private async Task<bool> PhoneNotExists(string phone, CancellationToken ct)
        {
            return !await _context.ApplicationUsers
                .AnyAsync(u => u.PhoneNumber == phone, ct);
        }

        private async Task<bool> CareerFieldExists(Guid careerFieldId, CancellationToken ct)
        {
            return await _context.CareerFields
                .AnyAsync(cf => cf.Id == careerFieldId, ct);
        }
    }
}