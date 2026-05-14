using BaridikExpress.Application.Features.Auth.DTO.Auth;
using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.RefreshToken
{
 
        public record RefreshTokenCommand(string RefreshToken)
            : IRequest<Result<RefreshTokenResponseDto>>;
    }

