using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.Logout
{
    public record LogoutCommand(string RefreshToken)
        : IRequest<Result<bool>>;
}