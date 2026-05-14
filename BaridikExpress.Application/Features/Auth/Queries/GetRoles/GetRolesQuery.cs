using BaridikExpress.Application.Features.Auth.DTO.Auth;


namespace BaridikExpress.Application.Features.Auth.Queries.GetRoles
{
    public record GetRolesQuery() : IRequest<Result<List<RoleDto>>>;
}
