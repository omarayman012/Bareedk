namespace BaridikExpress.Application.Features.Auth.Commands.CreateRole
{
    public record CreateRoleCommand(
       string name
    ) : IRequest<Result<string>>;
}
