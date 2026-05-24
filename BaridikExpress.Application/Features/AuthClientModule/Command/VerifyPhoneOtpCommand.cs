using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Command
{
    public class VerifyPhoneOtpCommand : IRequest<Result<string>>
    {
        public string PhoneNumber { get; set; } = default!;
        public string Otp { get; set; } = default!;
    }
}
