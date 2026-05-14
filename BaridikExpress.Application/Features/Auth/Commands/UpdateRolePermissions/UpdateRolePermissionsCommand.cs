namespace BaridikExpress.Application.Features.Auth.Commands.UpdateRolePermissions
{
    public record UpdateRolePermissionsCommand(
        string RoleId,
        List<Guid> PermissionIds
    ) : IRequest<Result<string>>;
}