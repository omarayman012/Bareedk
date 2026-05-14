using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email)
        : IRequest<Result<bool>>;
}