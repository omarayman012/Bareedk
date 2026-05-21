namespace BaridikExpress.Application.Features.Auth.Commands.ToggleSubAdminEmployeeStatus
{
    public class ToggleSubAdminEmployeeStatusCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}