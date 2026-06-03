using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Command
{
    public class SendEmailOtpCommand : IRequest<Result<string>>
    {
        public string Email { get; set; } = string.Empty;
    }
}
