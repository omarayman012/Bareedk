namespace BaridikExpress.Application.Features.Auth.Commands.CreateSubAdminEmployee;

public class CreateSubAdminEmployeeValidator : AbstractValidator<CreateSubAdminEmployeeCommand>
{
    public CreateSubAdminEmployeeValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage(localizer["FullNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["FullNameMaxLength"]);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localizer["EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["EmailInvalid"])
            .MaximumLength(150)
            .WithMessage(localizer["EmailMaxLength"]);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage(localizer["PhoneNumberMaxLength"])
            .Matches(@"^\+?[0-9\s\-()]+$")
            .WithMessage(localizer["PhoneNumberInvalid"])
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage(localizer["GenderRequired"])
            .Must(x => x == "Male" || x == "Female")
            .WithMessage(localizer["GenderInvalid"]);

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage(localizer["RoleIdRequired"]);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(localizer["PasswordRequired"])
            .MinimumLength(8)
            .WithMessage(localizer["PasswordMinLength"])
            .Matches(@"[A-Z]")
            .WithMessage(localizer["PasswordUpperCase"])
            .Matches(@"[a-z]")
            .WithMessage(localizer["PasswordLowerCase"])
            .Matches(@"[0-9]")
            .WithMessage(localizer["PasswordDigit"])
            .Matches(@"[\W_]")
            .WithMessage(localizer["PasswordSpecialChar"]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(localizer["ConfirmPasswordRequired"])
            .Equal(x => x.Password)
            .WithMessage(localizer["PasswordsDoNotMatch"]);

        RuleFor(x => x.ProfileImage)
            .Must(file => file!.Length <= 2 * 1024 * 1024)
            .WithMessage(localizer["ProfileImageMaxSize"])
            .Must(file => new[] { "image/jpeg", "image/png" }.Contains(file!.ContentType))
            .WithMessage(localizer["ProfileImageInvalidType"])
            .When(x => x.ProfileImage != null);
    }
}