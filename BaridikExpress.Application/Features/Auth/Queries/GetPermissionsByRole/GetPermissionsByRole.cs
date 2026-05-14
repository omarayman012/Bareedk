using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetPermissionsByRole
{
    public record GetPermissionsByRoleQuery(string RoleId) : IRequest<Result<List<PermissionDto>>>;
}