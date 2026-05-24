using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Command
{
    public class VerifyEmailOtpCommand : IRequest<Result<string>>
    {
        public string Email { get; set; } = string.Empty;

        public string OTP { get; set; } = string.Empty;
    }
}
