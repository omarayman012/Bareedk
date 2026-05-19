namespace BaridikExpress.Application.Features.Auth.Commands.UpdateSubAdminEmployee;

public class UpdateSubAdminEmployeeValidator : AbstractValidator<UpdateSubAdminEmployeeCommand>
{
    public UpdateSubAdminEmployeeValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer["IdRequired"]);

        RuleFor(x => x.FullName)
            .MaximumLength(100)
            .WithMessage(localizer["FullNameMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(localizer["EmailInvalid"])
            .MaximumLength(150)
            .WithMessage(localizer["EmailMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage(localizer["PhoneNumberMaxLength"])
            .Matches(@"^\+?[0-9\s\-()]+$")
            .WithMessage(localizer["PhoneNumberInvalid"])
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Gender)
            .Must(x => x == "Male" || x == "Female")
            .WithMessage(localizer["GenderInvalid"])
            .When(x => !string.IsNullOrWhiteSpace(x.Gender));

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(localizer["RoleIdRequired"])
            .When(x => !string.IsNullOrWhiteSpace(x.RoleId));

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage(localizer["PasswordMinLength"])
            .Matches(@"[A-Z]")
            .WithMessage(localizer["PasswordUpperCase"])
            .Matches(@"[a-z]")
            .WithMessage(localizer["PasswordLowerCase"])
            .Matches(@"[0-9]")
            .WithMessage(localizer["PasswordDigit"])
            .Matches(@"[\W_]")
            .WithMessage(localizer["PasswordSpecialChar"])
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage(localizer["PasswordsDoNotMatch"])
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.ProfileImage)
            .Must(file => file!.Length <= 4 * 1024 * 1024)
            .WithMessage(localizer["ProfileImageMaxSize"])
            .Must(file => new[] { "image/jpeg", "image/png" }.Contains(file!.ContentType))
            .WithMessage(localizer["ProfileImageInvalidType"])
            .When(x => x.ProfileImage != null);
    }
}