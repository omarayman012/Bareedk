using BaridikExpress.Application.DTOs.LoginModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Command
{
    public class RefreshTokenCommand : IRequest<Result<LoginResponseDto>>
    {
        public string UserId { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
