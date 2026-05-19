namespace BaridikExpress.Application.Features.Auth.Commands.UpdateSubAdminEmployee
{
    public class UpdateSubAdminEmployeeCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? RoleId { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}