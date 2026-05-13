using BaridikExpress.Application.Commands.Users;

namespace BaridikExpress.Application.Validators.Users
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
