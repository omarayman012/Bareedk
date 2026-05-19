using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateSubAdminEmployee
{
    public class CreateSubAdminEmployeeCommand : IRequest<Result<SubAdminEmployeeResponse>>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public IFormFile? ProfileImage { get; set; }
    }
}