using BaridikExpress.Application.Features.AuthClientModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Validator
{
    public class RegisterClientValidator : AbstractValidator<RegisterClientCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public RegisterClientValidator(IApplicationDbContext context, IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer["FullNameRequired"])
                .MinimumLength(3).WithMessage(_localizer["FullNameMinLength"])
                .MaximumLength(150).WithMessage(_localizer["FullNameMaxLength"]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer["EmailRequired"])
                .EmailAddress().WithMessage(_localizer["InvalidEmail"])
                .MaximumLength(256).WithMessage(_localizer["EmailMaxLength"])
                .MustAsync(EmailNotExists).WithMessage(_localizer["EmailAlreadyExists"]);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer["PhoneRequired"])
                .Matches(@"^\+\d{5,20}$").WithMessage(_localizer["InvalidPhone"])
                .MustAsync(PhoneNotExists).WithMessage(_localizer["PhoneAlreadyExists"]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer["PasswordRequired"])
                .Must(BeStrongPassword).WithMessage(_localizer["PasswordInvalid"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_localizer["ConfirmPasswordRequired"])
                .Equal(x => x.Password).WithMessage(_localizer["PasswordNotMatch"]);

            // قواعد خاصة بالعميل
            RuleFor(x => x.CareerFieldId)
                .NotEmpty().WithMessage(_localizer["CareerFieldRequired"])
                .MustAsync(CareerFieldExists).WithMessage(_localizer["CareerFieldNotFound"]);

            RuleFor(x => x.CompanyName)
                .MaximumLength(200).WithMessage(_localizer["CompanyNameMaxLength"]);

            RuleFor(x => x.CompanyLink)
                .MaximumLength(500).WithMessage(_localizer["CompanyLinkMaxLength"])
                .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.CompanyLink))
                .WithMessage(_localizer["InvalidCompanyLink"]);

            // الموافقات
            RuleFor(x => x.AcceptTerms)
                .Equal(true).WithMessage(_localizer["MustAcceptTerms"]);

            RuleFor(x => x.AcceptPrivacy)
                .Equal(true).WithMessage(_localizer["MustAcceptPrivacy"]);
        }

        // دوال مساعدة
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
            => !await _context.ApplicationUsers.AnyAsync(u => u.Email == email, ct);

        private async Task<bool> PhoneNotExists(string phone, CancellationToken ct)
            => !await _context.ApplicationUsers.AnyAsync(u => u.PhoneNumber == phone, ct);

        private async Task<bool> CareerFieldExists(Guid careerFieldId, CancellationToken ct)
            => await _context.CareerFields.AnyAsync(cf => cf.Id == careerFieldId, ct);
    }
}
