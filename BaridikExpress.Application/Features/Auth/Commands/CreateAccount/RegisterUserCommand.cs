using BaridikExpress.Application.Features.Auth.DTO.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateAccount
{
    public record RegisterUserCommand(
    string FullName,
    string Email,
    string Phone,
    string Password,
    string ConfirmPassword
) : IRequest<Result<UserResponseDto>>;
}
