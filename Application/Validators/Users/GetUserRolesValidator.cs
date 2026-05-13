namespace BaridikExpress.Application.Validators.Users
{
    public class GetUserRolesValidator : AbstractValidator<GetUserRolesQuery>
    {
        public GetUserRolesValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
