using FluentValidation;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.CreateFAQ;

public sealed class CreateFAQCommandValidator : AbstractValidator<CreateFAQCommand>
{
    public CreateFAQCommandValidator(IStringLocalizer localizer)
    {
        #region Question

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.QuestionAr) || !string.IsNullOrWhiteSpace(x.QuestionEn))
            .WithMessage(localizer["AtLeastOneQuestionRequired"])
            .OverridePropertyName("Question");

        When(x => !string.IsNullOrWhiteSpace(x.QuestionAr), () =>
            RuleFor(x => x.QuestionAr).MaximumLength(500).WithMessage(localizer["QuestionArMaxLength"]));

        When(x => !string.IsNullOrWhiteSpace(x.QuestionEn), () =>
            RuleFor(x => x.QuestionEn).MaximumLength(500).WithMessage(localizer["QuestionEnMaxLength"]));

        #endregion

        #region Answer

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.AnswerAr) || !string.IsNullOrWhiteSpace(x.AnswerEn))
            .WithMessage(localizer["AtLeastOneAnswerRequired"])
            .OverridePropertyName("Answer");

        #endregion
    }
}