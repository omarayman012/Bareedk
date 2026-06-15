using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Import;

public sealed class ImportServiceBusinessPlansCommandValidator
    : AbstractValidator<ImportServiceBusinessPlansCommand>
{
    public ImportServiceBusinessPlansCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage(localizer["FileIsRequired"]);

        When(x => x.File is not null, () =>
        {
            RuleFor(x => x.File)
                .Must(f => f!.Length > 0)
                .WithMessage(localizer["InvalidFile"]);

            RuleFor(x => x.File)
                .Must(f => Path.GetExtension(f!.FileName)
                    .Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                .WithMessage(localizer["OnlyExcelFilesAllowed"]);
        });
    }
}