using BaridikExpress.Application.DTO.AuthModules;
using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
 
        public record RefreshTokenCommand(string RefreshToken)
            : IRequest<Result<RefreshTokenResponseDto>>;
    }

