using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateCity;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.NameAr)
            .MaximumLength(100)
            .WithMessage(localizer["NameArMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameAr));

        RuleFor(x => x.NameEn)
            .MaximumLength(100)
            .WithMessage(localizer["NameEnMaxLength"])
            .When(x => !string.IsNullOrWhiteSpace(x.NameEn));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameAr)
                    || !string.IsNullOrWhiteSpace(x.NameEn)
                    || x.GovernmentId != Guid.Empty
                    || x.CountryId.HasValue)  
            .WithMessage(localizer["AtLeastOneFieldRequired"]);
    }
}