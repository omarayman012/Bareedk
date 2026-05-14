using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email, string OTP)
         : IRequest<Result<bool>>;
}

