using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Command
{
    public class ChangePasswordCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; } = default!;

        public string CurrentPassword { get; set; } = default!;

        public string NewPassword { get; set; } = default!;

        public string ConfirmPassword { get; set; } = default!;
    }
}
