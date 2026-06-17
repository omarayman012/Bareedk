using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.DTOs.LoginModule;
using MediatR;

namespace BaridikExpress.Application.Features.Auth.Command
{

    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public string EmailOrPhone { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

}