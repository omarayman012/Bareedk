namespace BaridikExpress.Application.Features.OurPlans.Commands.UploadPlans;

public sealed class UploadPlansCommandValidator
    : AbstractValidator<UploadPlansCommand>
{
    public UploadPlansCommandValidator(
        IStringLocalizer localizer)
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage(localizer["ExcelFileRequired"]);

        RuleFor(x => x.File)
            .Must(file =>
            {
                var extension = Path
                    .GetExtension(file!.FileName)
                    .ToLowerInvariant();

                return extension is ".xlsx" or ".xls";
            })
            .WithMessage(localizer["ExcelInvalidType"])
            .When(x => x.File != null);
    }
}