using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Delete;

public sealed class DeleteTalkServicesCommandValidator
    : AbstractValidator<DeleteTalkServicesCommand>
{
    public DeleteTalkServicesCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .WithMessage(localizer["IdsIsRequired"])

            .NotEmpty()
            .WithMessage(localizer["IdsIsRequired"])

            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage(localizer["DuplicateIdsAreNotAllowed"]);

        RuleForEach(x => x.Ids)
            .NotEqual(Guid.Empty)
            .WithMessage(localizer["IdIsInvalid"]);
    }
}