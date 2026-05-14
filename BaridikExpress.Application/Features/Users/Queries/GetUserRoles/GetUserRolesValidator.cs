namespace BaridikExpress.Application.Features.Users.Queries.GetUserRoles
{
    public class GetUserRolesValidator : AbstractValidator<GetUserRolesQuery>
    {
        public GetUserRolesValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
