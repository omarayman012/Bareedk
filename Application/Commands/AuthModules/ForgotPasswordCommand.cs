using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record ForgotPasswordCommand(string Email)
        : IRequest<Result<bool>>;
}