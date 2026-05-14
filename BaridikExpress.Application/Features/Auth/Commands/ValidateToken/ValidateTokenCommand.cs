using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Commands.ValidateToken
{
    public record ValidateTokenCommand() : IRequest<Result<ValidateTokenResponseDto>>;
}