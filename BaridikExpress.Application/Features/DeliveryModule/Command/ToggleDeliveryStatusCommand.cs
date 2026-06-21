using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Command
{
    public class ToggleDeliveryStatusCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
