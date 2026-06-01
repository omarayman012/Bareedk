using FluentValidation;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateFAQ;

public sealed class UpdateFAQCommandValidator : AbstractValidator<UpdateFAQCommand>
{
    public UpdateFAQCommandValidator(IStringLocalizer localizer)
    {
        #region Id

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(localizer["IdIsRequired"])
            .Must(id => id != Guid.Empty).WithMessage(localizer["InvalidId"]);

        #endregion

        #region Question (if sent)

        When(x => x.QuestionAr is not null, () =>
            RuleFor(x => x.QuestionAr)
                .NotEmpty().WithMessage(localizer["QuestionArIsRequired"])
                .MaximumLength(500).WithMessage(localizer["QuestionArMaxLength"]));

        When(x => x.QuestionEn is not null, () =>
            RuleFor(x => x.QuestionEn)
                .NotEmpty().WithMessage(localizer["QuestionEnIsRequired"])
                .MaximumLength(500).WithMessage(localizer["QuestionEnMaxLength"]));

        #endregion
    }
}