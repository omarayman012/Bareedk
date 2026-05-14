using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword)
        : IRequest<Result<bool>>;
}