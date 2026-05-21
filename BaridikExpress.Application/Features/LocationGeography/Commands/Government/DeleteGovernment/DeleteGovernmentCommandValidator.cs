namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.DeleteGovernment;

public class DeleteGovernmentCommandValidator
    : AbstractValidator<DeleteGovernmentCommand>
{
    public DeleteGovernmentCommandValidator(IStringLocalizer localizer)
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