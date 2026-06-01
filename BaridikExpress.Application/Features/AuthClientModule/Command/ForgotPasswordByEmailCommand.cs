using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Command
{
    public class ForgotPasswordByEmailCommand : IRequest<Result<string>>
    {
        public string Email { get; set; } = default!;
    }
}
