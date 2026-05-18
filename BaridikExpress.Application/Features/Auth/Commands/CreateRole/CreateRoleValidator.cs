namespace BaridikExpress.Application.Features.Auth.Commands.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer["RoleNameRequired"])
            .MaximumLength(50)
            .WithMessage(localizer["RoleNameTooLong"]);

        RuleFor(x => x.PermissionIds)
            .NotEmpty()
            .WithMessage(localizer["PermissionsRequired"])
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage(localizer["InvalidPermissionIds"]);
    }
}