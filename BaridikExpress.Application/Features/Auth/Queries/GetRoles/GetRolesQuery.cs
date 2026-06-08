using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetRoles;

public record GetRolesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Name = null
) : IRequest<Result<PaginatedList<RoleDto>>>;