namespace BaridikExpress.Application.Features.Users.Queries.GetUserRoles
{
    public record GetUserRolesQuery(string UserId) : IRequest<Result<List<string>>>;
}
