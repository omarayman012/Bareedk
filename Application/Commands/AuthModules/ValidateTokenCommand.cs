using BaridikExpress.Application.DTO.Auth;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record ValidateTokenCommand() : IRequest<Result<ValidateTokenResponseDto>>;
}