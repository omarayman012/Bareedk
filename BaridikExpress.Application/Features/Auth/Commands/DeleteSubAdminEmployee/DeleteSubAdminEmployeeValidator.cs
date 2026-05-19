namespace BaridikExpress.Application.Features.Auth.Commands.DeleteSubAdminEmployee;

public class DeleteSubAdminEmployeeValidator : AbstractValidator<DeleteSubAdminEmployeeCommand>
{
    public DeleteSubAdminEmployeeValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .WithMessage(localizer["IdsRequired"])
            .Must(ids => ids.Any())
            .WithMessage(localizer["IdsRequired"])
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage(localizer["InvalidId"])
            .Must(ids => ids.Count == ids.Distinct().Count())
            .WithMessage(localizer["DuplicateIds"]);
    }
}