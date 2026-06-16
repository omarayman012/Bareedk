using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Create;

public sealed class CreateTalkServiceCommandValidator
    : AbstractValidator<CreateTalkServiceCommand>
{
    public CreateTalkServiceCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.ServiceBusinessPlanIds)
            .NotEmpty()
            .WithMessage(localizer["ServiceBusinessPlanIdsIsRequired"])
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage(localizer["DuplicateServiceBusinessPlanIds"]);

        RuleForEach(x => x.ServiceBusinessPlanIds)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["ServiceBusinessPlanIdIsInvalid"]);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(localizer["FirstNameIsRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["FirstNameTooLong"]);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(localizer["LastNameIsRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["LastNameTooLong"]);

        RuleFor(x => x.CountryId)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["CountryIdIsRequired"]);

        RuleFor(x => x.GovernmentId)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["GovernmentIdIsRequired"]);

        RuleFor(x => x.CityId)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["CityIdInvalid"])
            .When(x => x.CityId.HasValue);

        RuleFor(x => x.VillageId)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["VillageIdInvalid"])
            .When(x => x.VillageId.HasValue);

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage(localizer["PostalCodeIsRequired"])
            .MaximumLength(20)
            .WithMessage(localizer["PostalCodeTooLong"]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumberIsRequired"])
            .MaximumLength(20)
            .WithMessage(localizer["PhoneNumberTooLong"]);

        RuleFor(x => x.WorkEmail)
            .NotEmpty()
            .WithMessage(localizer["WorkEmailIsRequired"])
            .EmailAddress()
            .WithMessage(localizer["WorkEmailInvalid"])
            .MaximumLength(200)
            .WithMessage(localizer["WorkEmailTooLong"]);

        RuleFor(x => x.JobTitle)
            .NotEmpty()
            .WithMessage(localizer["JobTitleIsRequired"])
            .MaximumLength(150)
            .WithMessage(localizer["JobTitleTooLong"]);

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage(localizer["CompanyNameIsRequired"])
            .MaximumLength(200)
            .WithMessage(localizer["CompanyNameTooLong"]);

        RuleFor(x => x.CompanyAddress)
            .NotEmpty()
            .WithMessage(localizer["CompanyAddressIsRequired"])
            .MaximumLength(500)
            .WithMessage(localizer["CompanyAddressTooLong"]);

        RuleFor(x => x.WebsiteUrl)
            .NotEmpty()
            .WithMessage(localizer["WebsiteUrlIsRequired"])
            .MaximumLength(300)
            .WithMessage(localizer["WebsiteUrlTooLong"])
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage(localizer["WebsiteUrlInvalid"]);

        RuleFor(x => x.AdditionalInformation)
            .NotEmpty()
            .WithMessage(localizer["AdditionalInformationIsRequired"])
            .MaximumLength(1000)
            .WithMessage(localizer["AdditionalInformationTooLong"]);
    }
}