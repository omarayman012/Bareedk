using BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    private readonly IStringLocalizer _localizer;

    public CreateCurrencyCommandValidator(IStringLocalizer localizer)
    {
        _localizer = localizer;

        RuleFor(x => x.NameEn)
            .MaximumLength(100).WithMessage(_localizer["NameMaxLength"]);

        RuleFor(x => x.NameAr)
            .MaximumLength(100).WithMessage(_localizer["NameMaxLength"]);

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.NameAr))
            .WithMessage(_localizer["NameRequired"]);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage(_localizer["CurrencyCodeRequired"])
            .MaximumLength(10).WithMessage(_localizer["CurrencyCodeMaxLength"]);

        RuleFor(x => x.CurrencySymbol)
            .MaximumLength(10).WithMessage(_localizer["CurrencySymbolMaxLength"]);
    }
}