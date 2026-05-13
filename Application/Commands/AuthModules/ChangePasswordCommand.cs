using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword)
        : IRequest<Result<bool>>;
}