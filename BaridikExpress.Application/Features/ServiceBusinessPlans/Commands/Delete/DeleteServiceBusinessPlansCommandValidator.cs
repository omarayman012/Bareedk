namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Delete;

public sealed class DeleteServiceBusinessPlansCommandValidator
    : AbstractValidator<DeleteServiceBusinessPlansCommand>
{
    public DeleteServiceBusinessPlansCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .WithMessage(localizer["IdsAreRequired"])
            .NotEmpty()
            .WithMessage(localizer["IdsAreRequired"]);

        RuleFor(x => x.Ids)
            .Must(ids => ids is not null && ids.Distinct().Count() == ids.Count)
            .WithMessage(localizer["DuplicateIdsAreNotAllowed"]);

        RuleForEach(x => x.Ids)
            .NotEmpty()
            .WithMessage(localizer["InvalidId"]);
    }
}