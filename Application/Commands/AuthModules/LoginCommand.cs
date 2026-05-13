using BaridikExpress.Application.DTO.Auth;
using BaridikExpress.Application.DTO.AuthModules;
using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record LoginCommand(string Email, string Password)
        : IRequest<Result<LoginResponseDto>>;
}