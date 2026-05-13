using BaridikExpress.Application.DTO.Auth;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record CreateRoleCommand(
       string name
    ) : IRequest<Result<string>>;
}
