using BaridikExpress.Application.DTO.Auth;


namespace BaridikExpress.Application.Queries.AuthModules
{
    public record GetRolesQuery() : IRequest<Result<List<RoleDto>>>;
}
