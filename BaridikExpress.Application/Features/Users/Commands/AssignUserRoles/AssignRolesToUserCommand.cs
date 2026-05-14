namespace BaridikExpress.Application.Features.Users.Commands.AssignUserRoles
{
        public record AssignRolesToUserCommand(
            string UserId,
            List<string> Roles
        ) : IRequest<Result<bool>>;
    }

