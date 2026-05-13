using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record LogoutCommand(string RefreshToken)
        : IRequest<Result<bool>>;
}