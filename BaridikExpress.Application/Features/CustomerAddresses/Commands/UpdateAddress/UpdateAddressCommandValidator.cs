using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.CustomerAddresses.Commands.UpdateAddress;

public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage(localizer["AddressIdRequired"]);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue)
            .WithMessage(localizer["InvalidLatitude"]);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue)
            .WithMessage(localizer["InvalidLongitude"]);
    }
}
