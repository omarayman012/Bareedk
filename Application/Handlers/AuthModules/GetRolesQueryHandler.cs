using BaridikExpress.Application.DTO.Auth;
using BaridikExpress.Application.Queries.AuthModules;

namespace BaridikExpress.Application.Handlers.AuthModules
{
    public class GetRolesQueryHandler(
        IApplicationDbContext context
    ) : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
    {
        public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await context.Roles
                .Select(r => new RoleDto(r.Id, r.Name!))
                .ToListAsync();

            return Result<List<RoleDto>>.Success(roles, "Operationcompletedsuccessfully", 200);
        }
    }
}
