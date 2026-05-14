using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmPassword)
        : IRequest<Result<bool>>;
}