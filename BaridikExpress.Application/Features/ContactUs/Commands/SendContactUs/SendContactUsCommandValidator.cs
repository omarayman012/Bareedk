using BaridikExpress.Application.Consts;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendContactUs;

public class SendContactUsCommandValidator : AbstractValidator<SendContactUsCommand>
{
    public SendContactUsCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["NameIsRequired"])
            .MaximumLength(200).WithMessage(localizer["NameMaxLength"]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailIsRequired"])
            .Matches(RegexPatterns.Email).WithMessage(localizer["InvalidEmail"]);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["PhoneIsRequired"])
            .MaximumLength(20).WithMessage(localizer["PhoneMaxLength"]);

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage(localizer["MessageIsRequired"])
            .MaximumLength(2000).WithMessage(localizer["MessageMaxLength"]);
    }
}