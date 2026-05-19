namespace BaridikExpress.Application.Features.Auth.Commands.DeleteSubAdminEmployee
{
    public class DeleteSubAdminEmployeeCommand : IRequest<Result<bool>>
    {
        public List<Guid> Ids { get; set; } = new();
    }
}