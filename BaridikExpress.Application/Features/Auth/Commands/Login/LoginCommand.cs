using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password)
        : IRequest<Result<LoginResponseDto>>;
}