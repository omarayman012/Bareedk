using BaridikExpress.Application.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record UpdateRoleCommand(
     string Id,
     string Name
    ) : IRequest<Result<string>>;
}
