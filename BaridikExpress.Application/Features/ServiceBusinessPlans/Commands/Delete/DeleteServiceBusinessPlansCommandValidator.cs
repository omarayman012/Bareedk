namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Delete;

public sealed class DeleteServiceBusinessPlansCommandValidator
: AbstractValidator<DeleteServiceBusinessPlansCommand>
{
    public DeleteServiceBusinessPlansCommandValidator(
    IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
        .NotEmpty()
        .WithMessage(localizer["IdsAreRequired"]);

    RuleFor(x => x.Ids)
        .Must(ids => ids.Distinct().Count() == ids.Count)
        .WithMessage(localizer["DuplicateIdsAreNotAllowed"]);

      
    }

}
