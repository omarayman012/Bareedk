namespace BaridikExpress.Application.Features.Auth.Commands.DeleteRole
{
    public record DeleteRoleCommand(string Id) : IRequest<Result<string>>;
}
