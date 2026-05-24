using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Command
{
    public class SendPhoneOtpCommand : IRequest<Result<string>>
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
