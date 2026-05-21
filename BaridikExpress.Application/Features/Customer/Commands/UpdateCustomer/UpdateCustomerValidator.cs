using FluentValidation;

namespace BaridikExpress.Application.Features.Customer.Commands.UpdateCustomer;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["CustomerIdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidCustomerId"]);

        #endregion

        #region Name (if sent)

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["NameIsRequired"])
                .MaximumLength(200).WithMessage(localizer["NameMaxLength"]);
        });

        #endregion

        #region Image (if sent)

        When(x => x.Image is not null, () =>
        {
            RuleFor(x => x.Image)
                .Must(file => file!.Length <= 5 * 1024 * 1024)
                .WithMessage(localizer["ImageMaxSize"])
                .Must(file => new[] { ".jpg", ".jpeg", ".png" }
                    .Contains(Path.GetExtension(file!.FileName).ToLower()))
                .WithMessage(localizer["ImageInvalidFormat"]);
        });

        #endregion

        #region Contacts (if sent)

        When(x => x.Contacts is { Count: > 0 }, () =>
        {
            RuleFor(x => x.Contacts)
                .Must(c => c!.Any(x => x.IsPrimary == true))
                .WithMessage(localizer["PrimaryContactIsRequired"]);

            RuleForEach(x => x.Contacts)
                .ChildRules(contact =>
                {
                    #region Phone

                    contact.When(x => x.PhoneCountryCode is not null, () =>
                    {
                        contact.RuleFor(x => x.PhoneCountryCode)
                            .NotEmpty().WithMessage(localizer["PhoneCountryCodeIsRequired"])
                            .MaximumLength(10).WithMessage(localizer["PhoneCountryCodeMaxLength"]);
                    });

                    contact.When(x => x.PhoneNumber is not null, () =>
                    {
                        contact.RuleFor(x => x.PhoneNumber)
                            .NotEmpty().WithMessage(localizer["PhoneNumberIsRequired"])
                            .MaximumLength(20).WithMessage(localizer["PhoneNumberMaxLength"]);
                    });

                    #endregion

                    #region Email

                    contact.When(x => x.Email is not null, () =>
                    {
                        contact.RuleFor(x => x.Email)
                            .EmailAddress().WithMessage(localizer["EmailInvalidFormat"])
                            .MaximumLength(256).WithMessage(localizer["EmailMaxLength"]);
                    });

                    #endregion

                    #region WhatsApp

                    contact.When(x => x.WhatsAppCountryCode is not null, () =>
                    {
                        contact.RuleFor(x => x.WhatsAppCountryCode)
                            .MaximumLength(10).WithMessage(localizer["WhatsAppCountryCodeMaxLength"]);
                    });

                    contact.When(x => x.WhatsAppNumber is not null, () =>
                    {
                        contact.RuleFor(x => x.WhatsAppNumber)
                            .MaximumLength(20).WithMessage(localizer["WhatsAppNumberMaxLength"]);
                    });

                    #endregion
                });
        });

        #endregion

        #region Addresses (if sent)

        When(x => x.Addresses is { Count: > 0 }, () =>
        {
            RuleForEach(x => x.Addresses!)
                .ChildRules(address =>
                {
                    #region Location Hierarchy

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

                    #region Optional Fields

                    address.When(x => x.Street is not null, () =>
                    {
                        address.RuleFor(x => x.Street)
                            .MaximumLength(300);
                    });

                    address.When(x => x.BuildingNumber is not null, () =>
                    {
                        address.RuleFor(x => x.BuildingNumber)
                            .MaximumLength(50);
                    });

                    address.When(x => x.FloorNumber is not null, () =>
                    {
                        address.RuleFor(x => x.FloorNumber)
                            .MaximumLength(50);
                    });

                    address.When(x => x.ZipCode is not null, () =>
                    {
                        address.RuleFor(x => x.ZipCode)
                            .MaximumLength(20);
                    });

                    #endregion
                });
        });

        #endregion

        #region Account (if sent)

        When(x => x.Account is not null, () =>
        {
            When(x => x.Account!.TaxRegistrationNumber is not null, () =>
            {
                RuleFor(x => x.Account!.TaxRegistrationNumber)
                    .MaximumLength(100).WithMessage(localizer["TaxRegistrationNumberMaxLength"]);
            });

            When(x => x.Account!.OpeningBalance is not null, () =>
            {
                RuleFor(x => x.Account!.OpeningBalance)
                    .GreaterThanOrEqualTo(0).WithMessage(localizer["OpeningBalanceMustBePositive"]);
            });

            When(x => x.Account!.OpeningBalance is not null && x.Account!.OpeningBalance > 0, () =>
            {
                RuleFor(x => x.Account!.OpeningBalanceDate)
                    .NotEmpty().WithMessage(localizer["OpeningBalanceDateIsRequired"])
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage(localizer["OpeningBalanceDateCannotBeInFuture"]);
            });
        });

        #endregion
    }
}