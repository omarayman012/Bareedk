using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.AuthCommand
{
    public class LogoutCommand : IRequest<Result<string>>
    {
        public string RefreshToken { get; set; }
    }
}
