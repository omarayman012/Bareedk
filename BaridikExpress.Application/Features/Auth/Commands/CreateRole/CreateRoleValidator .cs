namespace BaridikExpress.Application.Features.Auth.Commands.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.name)
                .NotEmpty()
              .WithMessage(localizer["Rolerequired"]);
        }
    }
}
