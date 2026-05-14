namespace BaridikExpress.Application.Features.Auth.Commands.UpdateRole
{
    public record UpdateRoleCommand(
     string Id,
     string Name
    ) : IRequest<Result<string>>;
}
