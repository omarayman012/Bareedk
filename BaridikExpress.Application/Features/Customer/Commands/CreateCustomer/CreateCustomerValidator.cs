using FluentValidation;

namespace BaridikExpress.Application.Features.Customer.Commands.CreateCustomer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator(IStringLocalizer localizer)
    {
        #region Name

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["NameIsRequired"])
            .MaximumLength(200).WithMessage(localizer["NameMaxLength"]);

        #endregion

        #region Image

        RuleFor(x => x.Image)
            .Must(file => file is null || file.Length <= 5 * 1024 * 1024)
            .WithMessage(localizer["ImageMaxSize"])
            .Must(file => file is null ||
                new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(localizer["ImageInvalidFormat"]);

        #endregion

        #region Password

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["PasswordIsRequired"])
            .MinimumLength(8).WithMessage(localizer["PasswordMinLength"])
            .Matches("[A-Z]").WithMessage(localizer["PasswordUppercase"])
            .Matches("[a-z]").WithMessage(localizer["PasswordLowercase"])
            .Matches("[0-9]").WithMessage(localizer["PasswordDigit"])
            .Matches("[^a-zA-Z0-9]").WithMessage(localizer["PasswordSpecialChar"]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(localizer["ConfirmPasswordIsRequired"])
            .Equal(x => x.Password).WithMessage(localizer["PasswordsDoNotMatch"]);

        #endregion

        #region Contacts

        RuleFor(x => x.Contacts)
            .NotEmpty().WithMessage(localizer["ContactsIsRequired"])
            .Must(c => c != null && c.Any(x => x.IsPrimary))
            .WithMessage(localizer["PrimaryContactIsRequired"]);

        RuleForEach(x => x.Contacts)
            .ChildRules(contact =>
            {
                #region Phone

                contact.RuleFor(x => x.PhoneCountryCode)
                    .NotEmpty().WithMessage(localizer["PhoneCountryCodeIsRequired"])
                    .MaximumLength(10).WithMessage(localizer["PhoneCountryCodeMaxLength"]);

                contact.RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage(localizer["PhoneNumberIsRequired"])
                    .MaximumLength(20).WithMessage(localizer["PhoneNumberMaxLength"]);

                #endregion

                #region Email

                contact.RuleFor(x => x.Email)
                    .EmailAddress().WithMessage(localizer["EmailInvalidFormat"])
                    .MaximumLength(256).WithMessage(localizer["EmailMaxLength"])
                    .When(x => !string.IsNullOrWhiteSpace(x.Email));

                #endregion

                #region WhatsApp

                contact.RuleFor(x => x.WhatsAppCountryCode)
                    .MaximumLength(10).WithMessage(localizer["WhatsAppCountryCodeMaxLength"])
                    .When(x => !string.IsNullOrWhiteSpace(x.WhatsAppCountryCode));

                contact.RuleFor(x => x.WhatsAppNumber)
                    .MaximumLength(20).WithMessage(localizer["WhatsAppNumberMaxLength"])
                    .When(x => !string.IsNullOrWhiteSpace(x.WhatsAppNumber));

                #endregion
            });

        #endregion

        #region Addresses

        When(x => x.Addresses is not null, () =>
        {
            RuleForEach(x => x.Addresses!)
                .ChildRules(address =>
                {
                    #region Location Hierarchy (Country → Government → City → Village)

                    address.RuleFor(x => x.CountryId)
                        .NotEmpty()
                        .When(x =>
                            x.GovernmentId.HasValue ||
                            x.CityId.HasValue ||
                            x.VillageId.HasValue ||
                            !string.IsNullOrWhiteSpace(x.Street));

                    address.RuleFor(x => x.GovernmentId)
                        .NotEmpty()
                        .When(x => x.CityId.HasValue || x.VillageId.HasValue);

                    address.RuleFor(x => x.CityId)
                        .NotEmpty()
                        .When(x => x.VillageId.HasValue);

                    #endregion

                    #region Street & Building

                    address.RuleFor(x => x.Street)
                        .NotEmpty().MaximumLength(300)
                        .When(x => x.CountryId.HasValue);

                    address.RuleFor(x => x.BuildingNumber)
                        .MaximumLength(50)
                        .When(x => !string.IsNullOrWhiteSpace(x.BuildingNumber));

                    #endregion

                    #region Optional Fields

                    address.RuleFor(x => x.FloorNumber)
                        .MaximumLength(50)
                        .When(x => !string.IsNullOrWhiteSpace(x.FloorNumber));

                    address.RuleFor(x => x.ZipCode)
                        .MaximumLength(20)
                        .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));

                    #endregion
                });
        });

        #endregion

        #region Account

        When(x => x.Account is not null, () =>
        {
            RuleFor(x => x.Account!.TaxRegistrationNumber)
                .MaximumLength(100).WithMessage(localizer["TaxRegistrationNumberMaxLength"])
                .When(x => !string.IsNullOrWhiteSpace(x.Account!.TaxRegistrationNumber));

            RuleFor(x => x.Account!.OpeningBalance)
                .GreaterThanOrEqualTo(0).WithMessage(localizer["OpeningBalanceMustBePositive"])
                .When(x => x.Account!.OpeningBalance is not null);

            RuleFor(x => x.Account!.OpeningBalanceDate)
                .NotEmpty().WithMessage(localizer["OpeningBalanceDateIsRequired"])
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(localizer["OpeningBalanceDateCannotBeInFuture"])
                .When(x => x.Account!.OpeningBalance is not null && x.Account!.OpeningBalance > 0);
        });

        #endregion
    }
}