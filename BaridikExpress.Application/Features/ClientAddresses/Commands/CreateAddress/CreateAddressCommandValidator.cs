namespace BaridikExpress.Application.Features.ClientAddresses.Commands.CreateAddress;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator(IStringLocalizer localizer)
    {
        // Latitude/Longitude range validation when provided
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue)
            .WithMessage(localizer["InvalidLatitude"]);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue)
            .WithMessage(localizer["InvalidLongitude"]);
    
        RuleFor(x => x.RecipientName)
            .NotEmpty()
            .WithMessage(localizer["RecipientNameRequired"]);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(localizer["EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["InvalidEmail"]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumberRequired"]);

        RuleFor(x => x.AddressType)
            .NotNull()
            .WithMessage(localizer["AddressTypeRequired"]);

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage(localizer["CountryRequired"]);

        RuleFor(x => x.GovernmentId)
            .NotEmpty()
            .WithMessage(localizer["GovernmentRequired"]);

        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage(localizer["CityRequired"]);

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(localizer["StreetRequired"]);

        RuleFor(x => x.BuildingNumber)
            .NotEmpty()
            .WithMessage(localizer["BuildingNumberRequired"]);

        RuleFor(x => x.FlatNumber)
            .NotEmpty()
            .WithMessage(localizer["FlatNumberRequired"]);

        RuleFor(x => x.Latitude)
            .NotNull()
            .WithMessage(localizer["LatitudeRequired"]);

        RuleFor(x => x.Longitude)
            .NotNull()
            .WithMessage(localizer["LongitudeRequired"]);
    }
}