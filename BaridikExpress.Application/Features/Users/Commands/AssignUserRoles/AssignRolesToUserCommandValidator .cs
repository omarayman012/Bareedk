namespace BaridikExpress.Application.Features.Users.Commands.AssignUserRoles
{
        public class AssignRolesToUserValidator : AbstractValidator<AssignRolesToUserCommand>
        {
            public AssignRolesToUserValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.Roles).NotEmpty();
            }
        }
    
}
