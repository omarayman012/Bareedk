using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.DeleteCity;

public class DeleteCityCommandValidator
    : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .WithMessage(localizer["IdsRequired"]);

        RuleFor(x => x.Ids)
            .NotEmpty()
            .WithMessage(localizer["IdsRequired"]);

        RuleFor(x => x.Ids)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage(localizer["DuplicateIdsNotAllowed"])
            .When(x => x.Ids != null && x.Ids.Count > 0);
    }
}