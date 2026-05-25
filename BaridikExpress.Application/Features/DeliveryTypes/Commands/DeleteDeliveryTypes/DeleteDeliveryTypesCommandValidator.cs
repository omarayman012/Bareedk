using FluentValidation;

namespace BaridikExpress.Application.Features.DeliveryTypes.Commands.DeleteDeliveryTypes;

public sealed class DeleteDeliveryTypesCommandValidator : AbstractValidator<DeleteDeliveryTypesCommand>
{
    public DeleteDeliveryTypesCommandValidator(IStringLocalizer localizer)
    {
        #region Ids

        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(localizer["IdsAreRequired"]);

        RuleFor(x => x.Ids)
            .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
            .WithMessage(localizer["InvalidIdInList"])
            .When(x => x.Ids is { Count: > 0 });

        RuleFor(x => x.Ids)
            .Must(ids => ids != null && ids.Distinct().Count() == ids.Count)
            .WithMessage(localizer["DuplicateIdsNotAllowed"])
            .When(x => x.Ids is { Count: > 0 });

        #endregion
    }
}